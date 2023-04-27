using System.ComponentModel.DataAnnotations;
using BlazorAnnuaireProject.Shared;

// DTO = Data Transfer Object = Objet de transfert de données
// C'est un objet qui permet de transférer des données entre le client et le serveur
public class SiteWithServicesDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom du site est obligatoire")]
    public string Ville { get; set; }
    public string Description { get; set; }
    public List<ServiceDto> Services { get; set; }
}

public class SiteWithSalariesDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom du site est obligatoire")]
    public string Ville { get; set; }
    public string Description { get; set; }
    public List<SalariesDto> Salaries { get; set; }

    public List<ServiceDto> Services { get; set; }

}

public class SiteDto {
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom du site est obligatoire")]
    public string Ville { get; set; }
    public string Description { get; set; }
}
