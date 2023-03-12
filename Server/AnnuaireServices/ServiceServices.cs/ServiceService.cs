using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.AnnuaireServices.ServiceService;

public class ServiceService : IServiceService
{
    private readonly DataContext _context;

    public ServiceService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Services>> GetAllServices()
    {
        return await _context.Services.Include(s => s.Salaries).ToListAsync();
    }

    public async Task<Services> GetServiceById(int id)
    {
        return await _context.Services.Include(s => s.Salaries).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> CreateService(Services service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service.Id;
    }

    public async Task UpdateService(Services service)
    {
        _context.Entry(service).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }
}

