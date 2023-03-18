using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;
public interface ISiteService
{

    // CRUD Site
    Task<List<Site>> GetAllSites();
    Task<List<Site>> GetAllSitesWithSalaries();


    List<Site> GetSitesWithServices(string ville);
    Task<Site> GetSiteByNameAndSalaries(string ville);
    Task<Site> GetSiteByNameAndSalariesAndServices(string ville);
    Task<Site> GetSiteByName(string ville);
    CreateSiteResponse CreateSite(CreateSiteRequest site);

    UpdateResponse UpdateSite(UpdateRequest site, string ville);
    DeleteResponse DeleteSite(string ville);
}