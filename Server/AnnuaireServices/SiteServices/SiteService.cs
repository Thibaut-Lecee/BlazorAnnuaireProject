using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Context;
using Microsoft.EntityFrameworkCore;
namespace BlazorAnnuaireProject.AnnuaireServices.SiteService;

public class SiteService : ISiteService
{

    private readonly DataContext _context;

    public SiteService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Site>> GetAllSites()
    {
        return await _context.Sites.Include(s => s.Salaries).ToListAsync();
    }

    public async Task<Site> GetSiteById(int id)
    {
        return await _context.Sites.Include(s => s.Salaries).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> CreateSite(Site site)
    {
        _context.Sites.Add(site);
        await _context.SaveChangesAsync();
        return site.Id;
    }

    public async Task UpdateSite(Site site)
    {
        _context.Entry(site).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSite(int id)
    {
        var site = await _context.Sites.FindAsync(id);
        _context.Sites.Remove(site);
        await _context.SaveChangesAsync();
    }

}