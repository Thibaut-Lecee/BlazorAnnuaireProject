using BlazorAnnuaireProject.AnnuaireServices.SiteService;
using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;
public interface ISalarieService
{
    Task<List<Salaries>> GetAllSalariesWithAssociations();
    Task<List<Salaries>> GetAllSalaries();
    Task<SalariesDto> GetSalariesById(int id);
    Task<SalariesDto> GetSalariesByEmailWithAssociations(string email);
    Task<Salaries> GetSalariesByEmail(string email);

}