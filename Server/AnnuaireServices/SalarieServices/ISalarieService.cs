using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Shared;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;
public interface ISalarieService
{
    Task<List<SalariesDto>> GetAllSalaries();
    Task<SalariesDto> GetSalariesByEmailWithAssociations(string email);
    Task<Salaries> GetSalariesByEmail(string email);

}