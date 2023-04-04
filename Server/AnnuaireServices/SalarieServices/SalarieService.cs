using AutoMapper;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Shared;
using BlazorAnnuaireProject.Shared.Entities;
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



    public async Task<List<SalariesDto>> GetAllSalaries()
    {
        var salaries = await _context.Salaries
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
            .ToListAsync();

        return salaries;
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