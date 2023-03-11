

namespace BlazorAnnuaireProject.Entities;

public class Services {
    public int Id { get; set; }
    public string Nom { get; set; }

    // association avec les salariés

    public ICollection<Salaries> Salaries { get; set; }

}