using BlazorAnnuaireProject.Shared;
using System.ComponentModel.DataAnnotations;


// DTO = Data Transfer Object = Objet de transfert de données
// C'est un objet qui permet de transférer des données entre le client et le serveur
public class ServiceDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom du service est obligatoire")]
    public string Nom { get; set; }

    public List<SalariesDto> Salaries { get; set; }

}