using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Context;
using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Models;
using AutoMapper;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.Shared;

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


    public async Task<List<Site>> GetAllSites() => await _context.Sites.ToListAsync();
    public async Task<List<SiteWithSalariesDto>> GetAllSitesWithSalaries()
    {
        var sites = await _context.Sites
             .Include(s => s.Salaries)
             .Select(s => new SiteWithSalariesDto
             {
                 Id = s.Id,
                 Ville = s.Ville,
                 Description = s.Description,
                 Salaries = s.Salaries
                     .Select(ss => new SalariesDto
                     {
                         Id = ss.Id,
                         Nom = ss.Nom,
                         Prenom = ss.Prenom,
                         Email = ss.Email,
                         CreatedAt = ss.CreatedAt,
                         TelephoneFixe = ss.TelephoneFixe,
                         TelephonePortable = ss.TelephonePortable,
                         Service = ss.Service.Nom,
                         Site = ss.Site.Ville
                     })
                     .ToList()
             })
             .ToListAsync();
        return sites;
    }


    public async Task<Site> GetSiteByName(string ville)
    {
        return await _context.Sites.FirstOrDefaultAsync(s => s.Ville == ville);
    }

    public async Task<List<SiteWithServicesDto>> GetSiteByNameAndServices(string ville)
    {
        var sites = await _context.Sites
            .Where(s => s.Ville == ville)
            .Include(s => s.SiteAndServices)
                .ThenInclude(ss => ss.Service)
            .Select(s => new SiteWithServicesDto
            {
                Id = s.Id,
                Ville = s.Ville,
                Description = s.Description,
                Services = s.SiteAndServices
                    .Select(ss => new ServiceDto
                    {
                        Id = ss.Service.Id,
                        Nom = ss.Service.Nom
                    })
                    .ToList()
            })
            .ToListAsync();

        return sites;
    }

    public async Task<List<SiteWithSalariesDto>> GetSiteByNameAndSalaries(string ville)
    {
        var sites = await _context.Sites
            .Where(s => s.Ville == ville)
            .Include(s => s.Salaries)
            .Select(s => new SiteWithSalariesDto
            {
                Id = s.Id,
                Ville = s.Ville,
                Description = s.Description,
                Salaries = s.Salaries
                    .Select(ss => new SalariesDto
                    {
                        Id = ss.Id,
                        Nom = ss.Nom,
                        Prenom = ss.Prenom,
                        Email = ss.Email,
                        CreatedAt = ss.CreatedAt,
                        TelephoneFixe = ss.TelephoneFixe,
                        TelephonePortable = ss.TelephonePortable,
                        Service = ss.Service.Nom,
                        Site = ss.Site.Ville
                    })
                    .ToList()
            })
            .ToListAsync();

        return sites;
    }


    public async Task<List<SiteWithSalariesDto>> GetSiteByNameAndSalariesAndServices(string ville)
    {

        var sites = await _context.Sites
            .Where(s => s.Ville == ville)
            .Include(s => s.Salaries)
            .Include(s => s.SiteAndServices)
                .ThenInclude(ss => ss.Service)
                .ThenInclude(s => s.Salaries)
            .Select(s => new SiteWithSalariesDto
            {
                Id = s.Id,
                Ville = s.Ville,
                Description = s.Description,
                Services = s.SiteAndServices
                    .Select(ss => new ServiceDto
                    {
                        Id = ss.Service.Id,
                        Nom = ss.Service.Nom,
                        Salaries = ss.Service.Salaries
                            .Select(s => new SalariesDto
                            {
                                Id = s.Id,
                                Nom = s.Nom,
                                Prenom = s.Prenom,
                                Email = s.Email,
                                CreatedAt = s.CreatedAt,
                                TelephoneFixe = s.TelephoneFixe,
                                TelephonePortable = s.TelephonePortable,
                                Service = s.Service.Nom,
                                Site = s.Site.Ville
                            })
                            .ToList()
                    })
                    .ToList(),
            })
            .ToListAsync();

        return sites;

    }





    public CreateSiteResponse CreateSite(CreateSiteRequest site)
    {
        // Vérifiez si le site existe déjà
        if (_context.Sites.Any(s => s.Ville == site.Ville))
        {
            throw new AppException(400, "Le site existe déjà");
        }
        var register = _mapper.Map<Site>(site);
        _context.Sites.Add(register);
        _context.SaveChanges();

        return new CreateSiteResponse(register);
    }

    public UpdateResponse UpdateSite(UpdateRequest site, int siteId)
    {
        var existingSite = _context.Sites.Find(siteId);
        if (existingSite == null)
        {
            throw new AppException(400, "Le site n'a pas été trouvé");
        }
        if (SalariesOnSite(existingSite.Id))
        {
            throw new AppException(400, "Impossible de modifier le site car il contient des salariés");
        }

        bool modified = false;

        // Vérifiez si le nom de la ville est différent de l'original et mettez à jour si nécessaire
        if (!string.IsNullOrWhiteSpace(site.Ville) && site.Ville != existingSite.Ville && !string.IsNullOrEmpty(site.Ville) && site.Ville != "string")
        {
            existingSite.Ville = site.Ville;
            modified = true;
        }

        // Vérifiez si la description est différente de l'original et mettez à jour si nécessaire
        if (!string.IsNullOrWhiteSpace(site.Description) && site.Description != existingSite.Description && !string.IsNullOrEmpty(site.Description) && site.Description != "string")
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

    public DeleteResponse DeleteSite(int id)
    {
        var site = _context.Sites.FirstOrDefault(s => s.Id == id);
        if (site == null)
        {
            throw new AppException(400, "Le site n'a pas été trouvé");
        }
        if (SalariesOnSite(site.Id))
        {
            throw new AppException(400, "Impossible de supprimer le site car il contient des salariés");
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
