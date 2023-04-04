using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Context.TypeConfigurations;
using BlazorAnnuaireProject.Models;
using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Shared;

namespace BlazorAnnuaireProject.Context;

public class DataContext : DbContext
{
    #region Constructors

    // on ajoute au dataContext la configuration pour pouvoir injecter la configuration dans le AdminEntityConfiguration
    public DataContext(DbContextOptions<DataContext> options, AdminConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    public AdminConfiguration Configuration { get; }

    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Admin>().HasOne(u => u.RefreshToken).WithOne(rt => rt.Admin).HasForeignKey<RefreshToken>(rt => rt.AdminId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ServicesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SitesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SalariesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SiteServiceEntityConfiguration());
        // permet d'injecter les données sensibles dans la base de données pour l'admin par le appsettings.json
        modelBuilder.ApplyConfiguration(new AdminEntityConfiguration(new AdminConfiguration
        {
            Email = Configuration.Email,
            Password = Configuration.Password
        }));



    }

    public DbSet<Site> Sites { get; set; }
    public DbSet<Services> Services { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Salaries> Salaries { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<SiteAndService> SiteAndServices { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }


}