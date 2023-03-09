

namespace BlazorAnnuaireProject.Entities;

public class Services {
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Description { get; set; }

    // association avec les salariés
    public List<Salaries> Salaries { get; set; }

    // association avec les sites
    public List<Site> Sites { get; set; }


}