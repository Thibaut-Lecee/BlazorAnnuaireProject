using AutoMapper;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;


public class SalarieService : ISalarieService
{
    private readonly DataContext _context;

    public SalarieService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Salaries>> GetAllSalariesWithAssociations()
    {
        return await _context.Salaries.Include(s => s.Service).Include(s => s.Site).ToListAsync();
    }

    public async Task<List<Salaries>> GetAllSalaries()
    {
        return await _context.Salaries.ToListAsync();
    }



    public async Task<Salaries> GetSalariesById(int id)
    {

        var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.Id == id);
        if (salary != null)
        {
            salary.Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == salary.ServiceId);
            salary.Site = await _context.Sites.FirstOrDefaultAsync(s => s.Id == salary.SiteId);
        }
        return salary;
    }

    

}