using BlazorAnnuaireProject.Models;
using BlazorAnnuaireProject.Shared.Entities;

public interface ISiteService
{

    // CRUD Site
    Task<List<Site>> GetAllSites();
    Task<List<SiteWithSalariesDto>> GetAllSitesWithSalaries();


    Task<List<SiteWithSalariesDto>> GetSiteByNameAndSalariesAndServices(string ville);
    Task<List<SiteWithSalariesDto>> GetSiteByNameAndSalaries(string ville);

    Task<List<SiteWithServicesDto>> GetSiteByNameAndServices(string ville);

    Task<Site> GetSiteByName(string ville);
    CreateSiteResponse CreateSite(CreateSiteRequest site);

    UpdateResponse UpdateSite(UpdateRequest site, string ville);
    DeleteResponse DeleteSite(string ville);
}