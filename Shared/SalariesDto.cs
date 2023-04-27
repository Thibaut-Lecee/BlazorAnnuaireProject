using System.ComponentModel.DataAnnotations;

namespace BlazorAnnuaireProject.Shared
{
    // DTO = Data Transfer Object = Objet de transfert de données
// C'est un objet qui permet de transférer des données entre le client et le serveur
    public class SalariesDto
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        [StringLength(10, ErrorMessage = "Le numéro de téléphone fixe doit contenir 10 chiffres")]
        public string? TelephoneFixe { get; set; }
        [StringLength(10, ErrorMessage = "Le numéro de téléphone portable doit contenir 10 chiffres")]
        public string? TelephonePortable { get; set; }
        public string Service { get; set; }
        public string Site { get; set; }
    }
}