using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Authorization;
using Microsoft.Extensions.Options;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.Models;
using AutoMapper;

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

        if (admin == null) throw new AppException("No admin found");
        Console.WriteLine(admin.PasswordHash, model.Password);

        if (!BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            throw new AppException("Password is incorrect");

        var AccessToken = _jwtUtils.GenerateAccessToken(admin);

        _context.SaveChanges();

        return new AuthenticateResponse(admin, AccessToken.AccessToken, AccessToken.NewToken, AccessToken.AccessTokenExpires, AccessToken.NewTokenExpires);
    }

    public void RegisterSalaries(RegisterRequest request)
    {
        if (_context.Salaries.Any(x => x.Email == request.Email))
            throw new AppException("Email " + request.Email + " is already taken");

        var register = _mapper.Map<Salaries>(request);

        register.CreatedAt = DateTime.UtcNow;
        _context.Salaries.Add(register);
        _context.SaveChanges();



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