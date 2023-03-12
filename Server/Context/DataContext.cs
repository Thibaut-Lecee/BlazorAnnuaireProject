using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Context.TypeConfigurations;

namespace BlazorAnnuaireProject.Context;

public class DataContext : DbContext
{
    #region Constructors
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Admin>().HasOne(u => u.RefreshToken).WithOne(rt => rt.Admin).HasForeignKey<RefreshToken>(rt => rt.AdminId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ServicesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SitesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SalariesEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AdminEntityConfiguration());

    }

    public DbSet<Site> Sites { get; set; }
    public DbSet<Services> Services { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Salaries> Salaries { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }


}