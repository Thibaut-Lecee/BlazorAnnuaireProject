using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Context;
using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Models;
using AutoMapper;
using BlazorAnnuaireProject.Helpers;

namespace BlazorAnnuaireProject.AnnuaireServices.SiteService;

public class SiteService : ISiteService
{

    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public SiteService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<List<Site>> GetAllSites()
    {
        return await _context.Sites.ToListAsync();
    }
    public async Task<List<Site>> GetAllSitesWithSalaries()
    {
        return await _context.Sites.Include(s => s.Salaries).ToListAsync();
    }

    public async Task<List<Site>> GetAllSiteWithServices()
    {
        return await _context.Sites.Include(s => s.Services).ToListAsync();
    }

    public async Task<List<Site>> GetAllSiteWithSalariesAndServices()
    {
        return await _context.Sites.Include(s => s.Salaries).Include(s => s.Services).ToListAsync();
    }

    public async Task<Site> GetSiteByName(string ville)
    {
        return await _context.Sites.FirstOrDefaultAsync(s => s.Ville == ville);
    }

    public async Task<Site> GetSiteByNameAndServices(string ville)
    {
        return await _context.Sites.Include(s => s.Services).FirstOrDefaultAsync(s => s.Ville == ville);
    }

    public async Task<Site> GetSiteByNameAndSalaries(string ville)
    {
        return await _context.Sites.Include(s => s.Salaries).FirstOrDefaultAsync(s => s.Ville == ville);
    }

    public async Task<Site> GetSiteByNameAndSalariesAndServices(string ville)
    {
        var site = await _context.Sites
            .Where(s => s.Ville == ville)
            .Include(s => s.Salaries)
                .ThenInclude(s => s.Service)
            .FirstOrDefaultAsync();

        if (site == null)
        {
            throw new AppException("Le site n'a pas été trouvé");
        }

        site.Salaries = site.Salaries.Select(s => new Salaries
        {
            Id = s.Id,
            Nom = s.Nom,
            Prenom = s.Prenom,
            Email = s.Email,
            CreatedAt = s.CreatedAt,
            TelephoneFixe = s.TelephoneFixe,
            TelephonePortable = s.TelephonePortable,
            Service = new Services { Nom = s.Service.Nom },
            Site = s.Site != null ? new Site { Id = s.Site.Id, Ville = s.Site.Ville, Description = s.Site.Description } : null
        }).ToList();

        return site;
    }

    public CreateSiteResponse CreateSite(CreateSiteRequest site)
    {
        var register = _mapper.Map<Site>(site);
        _context.Sites.Add(register);
        _context.SaveChanges();

        return new CreateSiteResponse(register);
    }

    public UpdateResponse UpdateSite(UpdateRequest site, string ville)
    {
        var existingSite = _context.Sites.FirstOrDefault(s => s.Ville == ville);
        if (existingSite == null)
        {
            throw new AppException("Le site n'a pas été trouvé");
        }
        if (SalariesOnSite(existingSite.Id))
        {
            throw new AppException("Impossible de modifier le site car il contient des salariés");
        }

        bool modified = false;

        // Vérifiez si le nom de la ville est différent de l'original et mettez à jour si nécessaire
        if (!string.IsNullOrWhiteSpace(site.Ville) && site.Ville != existingSite.Ville && site.Ville != "" && site.Ville != "string")
        {
            existingSite.Ville = site.Ville;
            modified = true;
        }

        // Vérifiez si la description est différente de l'original et mettez à jour si nécessaire
        if (!string.IsNullOrWhiteSpace(site.Description) && site.Description != existingSite.Description && site.Description != "" && site.Description != "string")
        {
            existingSite.Description = site.Description;
            modified = true;
        }

        if (modified)
        {
            // Utilisez Entry pour marquer l'entité existante comme modifiée
            _context.Entry(existingSite).State = EntityState.Modified;
            _context.SaveChanges();

            return new UpdateResponse(existingSite, "Le site a été mis à jour");
        }
        else
        {
            return new UpdateResponse(existingSite, "Aucune modification n'a été apportée");
        }
    }

    public DeleteResponse DeleteSite(string ville)
    {
        var site = _context.Sites.FirstOrDefault(s => s.Ville == ville);
        if (site == null)
        {
            throw new AppException("Le site n'a pas été trouvé");
        }
        if (SalariesOnSite(site.Id))
        {
            throw new AppException("Impossible de supprimer le site car il contient des salariés");
        }
        var infoRemoved = site.Ville + " " + site.Description;
        _context.Sites.Remove(site);
        _context.SaveChanges();

        return new DeleteResponse(infoRemoved, "Le site a été supprimé");
    }


    private bool SalariesOnSite(int siteId)
    {
        Console.WriteLine(siteId);
        var site = _context.Sites.Include(s => s.Salaries).FirstOrDefault(s => s.Id == siteId);
        if (site != null && site.Salaries.Count > 0)
        {
            Console.WriteLine(site.Salaries);
            return true;
        }
        return false;
    }

}
