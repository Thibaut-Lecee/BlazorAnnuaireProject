using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Models;
using AutoMapper;
using BlazorAnnuaireProject.Shared;

namespace BlazorAnnuaireProject.AnnuaireServices.ServiceService;

public class ServiceService : IServiceService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ServiceService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<List<Services>> GetAllServices()
    {
        return await _context.Services.Include(s => s.Salaries).ToListAsync();
    }

    public async Task<Services> GetServiceById(int id)
    {
        return await _context.Services.Include(s => s.Salaries).FirstOrDefaultAsync(s => s.Id == id);
    }

    public CreateServiceResponse CreateService(CreateServiceRequest service)
    {
        // vérifier si le service existe déjà
        if (_context.Services.Any(s => s.Nom == service.Nom))
        {
            throw new Exception("Service already exists");
        }
        var registerService = _mapper.Map<Services>(service);
        _context.Services.Add(registerService);
        _context.SaveChanges();
        return new CreateServiceResponse(registerService);
    }

    public CreateServiceResponse CreateServiceOnSite(CreateServiceRequest service, string Ville)
    {
        var site = _context.Sites.FirstOrDefault(s => s.Ville == Ville);
        if (site == null)
        {
            site = new Site { Ville = Ville, Description = "Aucune description" };
            _context.Entry(site).State = EntityState.Added;
            _context.SaveChanges();

        }
        var serviceExist = _context.Services.FirstOrDefault(s => s.Nom == service.Nom);
        if(serviceExist == null) {
            serviceExist = _mapper.Map<Services>(service);
            _context.Services.Add(serviceExist);
        }

        if (site.SiteAndServices == null)
        {
            site.SiteAndServices = new List<SiteAndService>();
        }

        if (site.SiteAndServices.Any(s => s.Service.Nom == service.Nom))
        {
            throw new Exception("Service on Site already exists");
        }
      
        site.SiteAndServices.Add(new SiteAndService { Site = site, Service = serviceExist });

        _context.Entry(site).State = EntityState.Modified;
        _context.SaveChanges();
        return new CreateServiceResponse(serviceExist);
    }

    public UpdateServiceResponse UpdateService(UpdateServiceRequest service, int serviceId)
    {
        var existingService = _context.Services.FirstOrDefault(s => s.Id == serviceId);
        if (existingService == null)
        {
            throw new Exception("Service not found");
        }
        if (SalariesOnServices(existingService.Id))
        {
            throw new Exception("Service has salaries");
        }

        bool updated = false;
        if (!string.IsNullOrWhiteSpace(service.Nom) && !string.IsNullOrEmpty(service.Nom) && service.Nom != existingService.Nom && service.Nom != "string")
        {
            existingService.Nom = service.Nom;
            updated = true;
        }

        if (updated)
        {
            _context.Entry(existingService).State = EntityState.Modified;
            _context.SaveChanges();
            return new UpdateServiceResponse(existingService, "Service updated");
        }

        return new UpdateServiceResponse(existingService, "Service not updated");
    }

    public DeleteServiceResponse DeleteService(string nom)
    {
        var service = _context.Services.FirstOrDefault(s => s.Nom == nom);
        if (service == null)
        {
            throw new Exception("Service not found");
        }
        if (SalariesOnServices(service.Id))
        {
            throw new Exception("Service has salaries");
        }
        var infoRemoved = service.Nom;
        _context.Services.Remove(service);
        _context.SaveChangesAsync();
        return new DeleteServiceResponse(infoRemoved, "Service deleted");
    }

    private bool SalariesOnServices(int serviceId)
    {
        var site = _context.Services.Include(s => s.Salaries).FirstOrDefault(s => s.Id == serviceId);
        if (site != null && site.Salaries.Count > 0)
        {
            return true;
        }
        return false;
    }

}

