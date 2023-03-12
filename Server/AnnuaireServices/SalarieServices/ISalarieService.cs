using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;
public interface ISalarieService
{
    Task<List<Salaries>> GetAllSalaries();
    Task<Salaries> GetSalariesById(int id);
    Task<int> CreateSalaries(Salaries salaries);
    Task UpdateSalaries(Salaries salaries);
    Task DeleteSalaries(int id);
}