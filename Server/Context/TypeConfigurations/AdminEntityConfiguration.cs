using BlazorAnnuaireProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BlazorAnnuaireProject.Context.TypeConfigurations;
class AdminEntityConfiguration : IEntityTypeConfiguration<Admin>
{

    public void Configure(EntityTypeBuilder<Admin> builder)
    {

        var password = BCrypt.Net.BCrypt.HashPassword("Admin");
        builder.ToTable("Admins");
        builder.HasKey(admin => admin.AdminId);
        builder.HasData(new Admin
        {
            AdminId = 1,
            Nom = "Admin",
            Prenom = "Admin",
            Email = "leceethibaut@gmail.com",
            PasswordHash = password,
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