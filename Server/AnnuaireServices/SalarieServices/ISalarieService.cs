using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;
public interface ISalarieService
{
    Task<List<Salaries>> GetAllSalariesWithAssociations();
    Task<List<Salaries>> GetAllSalaries();
    Task<Salaries> GetSalariesById(int id);


}