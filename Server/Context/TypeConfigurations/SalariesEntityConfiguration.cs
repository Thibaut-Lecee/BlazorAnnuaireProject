namespace BlazorAnnuaireProject.Context.TypeConfigurations;
using BlazorAnnuaireProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bogus;
using System.Linq;

public class ServicesEntityConfiguration : IEntityTypeConfiguration<Services>
{
    public void Configure(EntityTypeBuilder<Services> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(s => s.Id);
        builder.HasData(
            new Services { Id = 1, Nom = "Comptabilité" },
            new Services { Id = 2, Nom = "Production" },
            new Services { Id = 3, Nom = "Accueil" },
            new Services { Id = 4, Nom = "Informatique" },
            new Services { Id = 5, Nom = "Commercial" }
        );
    }
}

public class SitesEntityConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("Sites");
        builder.HasKey(s => s.Id);

        builder.HasData(
            new Site { Id = 1, Description = "Siège administratif", Ville = "Paris" },
            new Site { Id = 2, Description = "Site de Production", Ville = "Nantes" },
            new Site { Id = 3, Description = "Site de Production", Ville = "Toulouse" },
            new Site { Id = 4, Description = "Site de Production", Ville = "Nice" },
            new Site { Id = 5, Description = "Site de Production", Ville = "Lille" }
        );
    }
}

public class SalariesEntityConfiguration : IEntityTypeConfiguration<Salaries>
{
    public void Configure(EntityTypeBuilder<Salaries> builder)
    {
        builder.ToTable("Salaries");
        builder.HasKey(s => s.Id);

        var faker = new Faker(locale: "fr");

        builder.HasData(Enumerable.Range(1, 1000).Select(i =>
            {
                var serviceId = faker.Random.Int(1, 5);
                var siteId = faker.Random.Int(1, 5);
                var prenom = faker.Name.FirstName();
                var nom = faker.Name.LastName();
                var email = faker.Internet.Email(prenom, nom);
                return new Salaries
                {
                    Id = i + 100,
                    Nom = nom,
                    Prenom = prenom,
                    Email = email,
                    TelephoneFixe = faker.Phone.PhoneNumber(),
                    TelephonePortable = faker.Phone.PhoneNumber(),
                    ServiceId = serviceId,
                    SiteId = siteId,
                    CreatedAt = faker.Date.Past()
                };
            }).ToArray());
    }
}