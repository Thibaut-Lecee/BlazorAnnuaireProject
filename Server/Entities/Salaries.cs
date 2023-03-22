using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Entities;

public class Salaries
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? TelephoneFixe { get; set; }
    public string? TelephonePortable { get; set; }

    //1ère association avec le Service  (1 salarié = 1 service)
    [JsonIgnore] public int? ServiceId { get; set; }
    public Services? Service { get; set; }

    // 2ème association avec le site  (1 salarié = 1 site)
    [JsonIgnore] public int? SiteId { get; set; }
    public Site? Site { get; set; }

}