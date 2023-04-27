using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Models;
using BlazorAnnuaireProject.Shared;

public interface IAdminService
{
    Task<List<Admin>> GetAllAdmins();
    Task<Admin> GetAdminsById(int id);
    AuthenticateResponse Login(AuthenticateRequest model);

    CreateSalarieReponse CreateSalarie(RegisterRequestSalarie salarie);
    CreateSalarieReponse CreateSalarieOnService(RegisterRequestSalarie salarie, string Service);
    CreateSalarieReponse CreateSalarieOnServiceAndSite(RegisterRequestSalarie salarie, string Site, string Service);
    UpdateSalarieResponse UpdateSalarie(UpdateSalarieRequest salarie, int salarieId);
    Admin GetAdminByRefreshToken(string token);
    DeleteResponse DeleteSalarie(string email);

}