using BlazorAnnuaireProject.Shared;
using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;
public interface ISalarieService
{
    Task<List<SalariesDto>> GetAllSalaries();
    Task<SalariesDto> GetSalariesByEmailWithAssociations(string email);
    Task<Salaries> GetSalariesByEmail(string email);

}