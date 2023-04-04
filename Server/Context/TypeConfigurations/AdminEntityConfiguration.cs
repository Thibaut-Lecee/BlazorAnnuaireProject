using Microsoft.EntityFrameworkCore;
using BlazorAnnuaireProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Context.TypeConfigurations;
class AdminEntityConfiguration : IEntityTypeConfiguration<Admin>
{
    private readonly AdminConfiguration _adminConfig;

    // on crée un constructeur pour pouvoir injecter la configuration afin de cacher les données sensibles pour l'admin
    public AdminEntityConfiguration(AdminConfiguration adminConfig)
    {
        _adminConfig = adminConfig;
    }
    public void Configure(EntityTypeBuilder<Admin> builder)
    {


        builder.ToTable("Admins");
        builder.HasKey(admin => admin.AdminId);
        builder.HasData(new Admin
        {
            AdminId = 1,
            Nom = "Admin",
            Prenom = "Admin",
            Email = _adminConfig.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(_adminConfig.Password),
            RoleId = 1,

        });
    }
}

class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(role => role.Id);
        builder.Property(role => role.Name).IsRequired();

        builder.HasData(new Role
        {
            Id = 1,
            Name = "Admin",
            Description = "Admin CRUD Requete"
        });
    }

}