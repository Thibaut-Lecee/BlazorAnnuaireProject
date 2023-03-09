namespace BlazorAnnuaireProject.Entities;


public class Site {
    public int Id { get; set; }

    public string Ville { get; set; }
    public string Description { get; set; }

    // association avec les services
    public List<Services> Services { get; set; }
}