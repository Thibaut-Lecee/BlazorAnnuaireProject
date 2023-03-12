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

    public async Task<List<Salaries>> GetAllSalaries()
    {
        return await _context.Salaries.Include(s => s.Service).Include(s => s.Site).ToListAsync();
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

    public async Task<int> CreateSalaries(Salaries salaries)
    {
        _context.Salaries.Add(salaries);
        await _context.SaveChangesAsync();
        return salaries.Id;
    }

    public async Task UpdateSalaries(Salaries salaries)
    {
        _context.Entry(salaries).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSalaries(int id)
    {
        var salaries = await _context.Salaries.FindAsync(id);
        _context.Salaries.Remove(salaries);
        await _context.SaveChangesAsync();
    }
}