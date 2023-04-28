using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Shared;
using BlazorAnnuaireProject.Authorization;
using Microsoft.Extensions.Options;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.Models;
using AutoMapper;
using System.Net;
using BlazorAnnuaireProject.Shared.Entities;

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

        // décryptage du mot de passe et vérification
        if (!BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            throw new AppException(400, "Password is incorrect");

        var AccessToken = _jwtUtils.GenerateAccessToken(admin);

        _context.SaveChanges();
        // renvoie des données de l'utilisateur avec le token d'authentification
        return new AuthenticateResponse(admin, AccessToken.AccessToken, AccessToken.NewToken, AccessToken.AccessTokenExpires, AccessToken.NewTokenExpires, admin.Role);
    }



    public CreateSalarieReponse CreateSalarie(RegisterRequestSalarie salarie)
    {
        if (_context.Salaries.Any(s => s.Email == salarie.Email))
        {
            throw new AppException(400, "Salarie already exists");
        }

        // mappage de l'entité salarie à partir du modèle de requête et enregistrement dans la base de données
        var registerSalarie = _mapper.Map<Salaries>(salarie);

        _context.Salaries.Add(registerSalarie);
        _context.SaveChanges();
        return new CreateSalarieReponse(registerSalarie);
    }



    // permet de créer un salarié avec un service et un site existant dans la base de données
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


    public UpdateSalarieResponse UpdateSalarie(UpdateSalarieRequest salarie, int salarieId)
    {
        var existingSalarie = _context.Salaries.FirstOrDefault(s => s.Id == salarieId);
        Console.WriteLine(existingSalarie);
        Console.WriteLine(salarieId);
        if (existingSalarie == null)
        {
            throw new Exception("Salarie not found");
        }
        var service = _context.Services.FirstOrDefault(s => s.Nom == salarie.Service);
        var site = _context.Sites.FirstOrDefault(s => s.Ville == salarie.Site);

        var duplicateSalarie = _context.Salaries
            .Any(s => s.Id != existingSalarie.Id && // Ajout de cette condition pour exclure le salarié actuel
                                                    // on regarde si un salarié existe avec les mêmes informations que le salarié à modifier dans le service et le site spécifié
                      s.Service.Id == service.Id &&
                      s.Site.Id == site.Id &&
                      (s.Nom == salarie.Nom || s.Prenom == salarie.Prenom || s.TelephoneFixe == salarie.TelephoneFixe || s.TelephonePortable == salarie.TelephonePortable));

        if (duplicateSalarie)
        {
            throw new Exception("A salarie with the same information already exists in the specified service and site.");
        }

        if (!string.IsNullOrEmpty(salarie.Nom))
        {
            existingSalarie.Nom = salarie.Nom;
        }
        if (!string.IsNullOrEmpty(salarie.Prenom))
        {
            existingSalarie.Prenom = salarie.Prenom;
        }
        if (!string.IsNullOrEmpty(salarie.Email))
        {
            existingSalarie.Email = salarie.Email;
        }
        if (!string.IsNullOrEmpty(salarie.TelephoneFixe))
        {
            existingSalarie.TelephoneFixe = salarie.TelephoneFixe;
        }
        if (!string.IsNullOrEmpty(salarie.TelephonePortable))
        {
            existingSalarie.TelephonePortable = salarie.TelephonePortable;
        }
        if (!string.IsNullOrEmpty(salarie.Service))
        {
            existingSalarie.Service = _context.Services.FirstOrDefault(s => s.Nom == salarie.Service) ?? existingSalarie.Service;
        }
        if (!string.IsNullOrEmpty(salarie.Site))
        {
            existingSalarie.Site = _context.Sites.FirstOrDefault(s => s.Ville == salarie.Site) ?? existingSalarie.Site;
        }
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


    public DeleteResponse DeleteSalariesOnService(int serviceId) {
        var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
        if (service == null)
        {
            throw new Exception("Service not found");
        }
        var removed = service.Nom;
        _context.Services.Remove(service);
        _context.SaveChanges();

        return new DeleteResponse(removed, "Le service a été supprimé avec tous les salariés associés");
    }

    public DeleteResponse DeleteSalariesOnSite(int SiteId)
    {
        var site = _context.Sites.FirstOrDefault(s => s.Id == SiteId);
        if (site == null)
        {
            throw new Exception("Site not found");
        }
        // il faut supprimer également les services associés au site
        var services = _context.SiteAndServices.Where(s => s.Site.Id == SiteId);
        foreach (var service in services)
        {
            _context.Services.Remove(service.Service);
        }
        var removed = site.Ville;
        _context.Sites.Remove(site);
        _context.SaveChanges();

        return new DeleteResponse(removed, "Le site a été supprimé avec tous les services et salariés associés");
}

public DeleteResponse DeleteAllSalaries()
{
    var salaries = _context.Salaries;
    foreach (var salarie in salaries)
    {
        _context.Salaries.Remove(salarie);
    }
    var removed = "Tous les salariés";
    _context.SaveChanges();

    return new DeleteResponse(removed, "ont été supprimés");
}

}