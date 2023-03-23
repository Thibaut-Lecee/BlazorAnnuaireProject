using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Authorization;
using Microsoft.Extensions.Options;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.Models;
using AutoMapper;
using System.Net;

namespace BlazorAnnuaireProject.AnnuaireServices.AdminService;
public class AdminService : IAdminService
{

    private readonly DataContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public AdminService(DataContext context, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
        _mapper = mapper;
    }


    public async Task<List<Admin>> GetAllAdmins()
    {
        return await _context.Admins.ToListAsync();
    }

    public async Task<Admin> GetAdminsById(int id)
    {
        return await _context.Admins.FindAsync(id);

    }

    public AuthenticateResponse Login(AuthenticateRequest model)
    {
        var admin = _context.Admins.SingleOrDefault(x => x.Email == model.Email);

        if (admin == null) throw new AppException(400, "No admin found");
        Console.WriteLine(admin.PasswordHash, model.Password);

        if (!BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            throw new AppException(400, "Password is incorrect");

        var AccessToken = _jwtUtils.GenerateAccessToken(admin);

        _context.SaveChanges();

        return new AuthenticateResponse(admin, AccessToken.AccessToken, AccessToken.NewToken, AccessToken.AccessTokenExpires, AccessToken.NewTokenExpires);
    }

    public CreateSalarieReponse CreateSalarie(RegisterRequestSalarie salarie)
    {
        if (_context.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new AppException(400, "Salarie already exists");
        }
        var registerSalarie = _mapper.Map<Salaries>(salarie);

        _context.Salaries.Add(registerSalarie);
        _context.SaveChanges();
        return new CreateSalarieReponse(registerSalarie);
    }


    public CreateSalarieReponse CreateSalarieOnServiceAndSite(RegisterRequestSalarie salarie, string Site, string Service)
    {
        if (_context.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new Exception("Salarie already exists");
        }
        var service = _context.Services.FirstOrDefault(s => s.Nom == Service);
        var site = _context.Sites.FirstOrDefault(s => s.Ville == Site);
        if (service == null || site == null)
        {
            throw new Exception("Service or Site not found");
        }
        if (service.Salaries == null || site.Salaries == null)
        {
            service.Salaries = new List<Salaries>();
            site.Salaries = new List<Salaries>();
        }
        if (service.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new Exception("Salarie already exists");
        }

        var registerSalarie = _mapper.Map<Salaries>(salarie);
        registerSalarie.Site = site;
        registerSalarie.Service = service;
        _context.Salaries.Add(registerSalarie);
        service.Salaries.Add(registerSalarie);
        site.Salaries.Add(registerSalarie);
        _context.Entry(service).State = EntityState.Modified;
        _context.SaveChanges();
        return new CreateSalarieReponse(registerSalarie);
    }


    public CreateSalarieReponse CreateSalarieOnService(RegisterRequestSalarie salarie, string Service)
    {
        if (_context.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new Exception("Salarie already exists");
        }
        var service = _context.Services.FirstOrDefault(s => s.Nom == Service);
        if (service == null)
        {
            throw new Exception("Service not found");
        }

        if (service.Salaries == null)
        {
            service.Salaries = new List<Salaries>();
        }

        if (service.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new Exception("Salarie already exists");
        }


        var registerSalarie = _mapper.Map<Salaries>(salarie);
        _context.Salaries.Add(registerSalarie);
        service.Salaries.Add(registerSalarie);
        _context.Entry(service).State = EntityState.Modified;
        _context.SaveChanges();
        return new CreateSalarieReponse(registerSalarie);
    }
    public UpdateSalarieResponse UpdateSalarie(UpdateSalarieRequest salarie)
    {
        var existingSalarie = _context.Salaries.FirstOrDefault(s => s.Email == salarie.Email);

        if (existingSalarie == null)
        {
            throw new Exception("Salarie not found");
        }

        if (_context.Salaries.Any(s => s.Email == salarie.Email) || _context.Salaries.Any(s => s.TelephoneFixe == salarie.TelephoneFixe)
            || _context.Salaries.Any(s => s.TelephonePortable == salarie.TelephonePortable))
        {
            throw new Exception("Salarie already exists with this email or phone number" + salarie.Email + " " + salarie.TelephoneFixe + " " + salarie.TelephonePortable);
        }

        existingSalarie.Nom = salarie.Nom;
        existingSalarie.Prenom = salarie.Prenom;
        existingSalarie.Email = salarie.Email;
        existingSalarie.TelephoneFixe = salarie.TelephoneFixe;
        existingSalarie.TelephonePortable = salarie.TelephonePortable;
        existingSalarie.Service = _context.Services.FirstOrDefault(s => s.Nom == salarie.Service);
        existingSalarie.Site = _context.Sites.FirstOrDefault(s => s.Ville == salarie.Site);
        _context.Entry(existingSalarie).State = EntityState.Modified;
        _context.SaveChanges();
        return new UpdateSalarieResponse(existingSalarie);
    }

    public DeleteResponse DeleteSalarie(string email)
    {
        var salarie = _context.Salaries.FirstOrDefault(s => s.Email == email);
        if (salarie == null)
        {
            throw new AppException(400, "Le salarie n'a pas été trouvé");
        }
        var removed = salarie.Email;
        _context.Salaries.Remove(salarie);
        _context.SaveChanges();

        return new DeleteResponse(removed, "Le site a été supprimé");
    }

    public Admin GetAdminByRefreshToken(string token)
    {
        Console.WriteLine("token: " + token);
        var admin = _context.Admins.Include(u => u.RefreshToken)
                              .FirstOrDefault(u => u.RefreshToken.AccessToken == token || u.RefreshToken.NewToken == token);
        if (admin == null) throw new KeyNotFoundException("Tous les tokens sont expirés");
        return admin;
    }




}