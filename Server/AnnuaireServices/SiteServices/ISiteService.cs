using BlazorAnnuaireProject.Entities;

public interface ISiteService
{

    // CRUD Site
    Task<List<Site>> GetAllSites();
    Task<Site> GetSiteById(int id);
    Task<int> CreateSite(Site site);
    Task UpdateSite(Site site);
    Task DeleteSite(int id);
}