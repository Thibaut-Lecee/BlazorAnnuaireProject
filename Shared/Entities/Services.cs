using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Shared.Entities;

public class Services
{
    public int Id { get; set; }
    public string Nom { get; set; }

    // association avec les salariés

    public ICollection<Salaries> Salaries { get; set; }

    public ICollection<SiteAndService> SiteAndServices { get; set; }

}