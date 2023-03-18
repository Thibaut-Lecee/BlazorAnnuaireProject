namespace BlazorAnnuaireProject.Entities;


public class Site
{
    public int Id { get; set; }

    public string Ville { get; set; }
    public string Description { get; set; }
    // association avec les salariés
    public ICollection<Salaries> Salaries { get; set; }

    // association avec les services
    public ICollection<SiteAndService> SiteAndServices { get; set; }

}