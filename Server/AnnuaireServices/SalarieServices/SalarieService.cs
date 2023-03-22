using AutoMapper;
using BlazorAnnuaireProject.AnnuaireServices.SiteService;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;
using Microsoft.EntityFrameworkCore;


namespace BlazorAnnuaireProject.AnnuaireServices.SalarieService;


public class SalarieService : ISalarieService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public SalarieService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Salaries>> GetAllSalariesWithAssociations()
    {
        return await _context.Salaries.Include(s => s.Service).Include(s => s.Site).ToListAsync();
    }

    public async Task<List<Salaries>> GetAllSalaries()
    {
        return await _context.Salaries.ToListAsync();
    }



    public async Task<SalariesDto> GetSalariesById(int id)
    {

        var salary = await _context.Salaries.Where(s => s.Id == id)
        .Include(s => s.Service)
          .Include(s => s.Site)
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
          .FirstOrDefaultAsync();

        return salary;
    }

    public async Task<SalariesDto> GetSalariesByEmailWithAssociations(string email)
    {
        var salary = await _context.Salaries.Where(s => s.Email == email)
        .Include(s => s.Service)
          .Include(s => s.Site)
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
          .FirstOrDefaultAsync();

        return salary;
    }

    public async Task<Salaries> GetSalariesByEmail(string email)
    {
        return await _context.Salaries.FirstOrDefaultAsync(s => s.Email == email);
    }












}