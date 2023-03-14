using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;
public interface IAdminService
{
    Task<List<Admin>> GetAllAdmins();
    Task<Admin> GetAdminsById(int id);
    AuthenticateResponse Login(AuthenticateRequest model);

    void RegisterSalaries(RegisterRequest request);

    Admin GetAdminByRefreshToken(string token);
}