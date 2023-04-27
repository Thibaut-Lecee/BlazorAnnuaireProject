using System.ComponentModel.DataAnnotations;

namespace BlazorAnnuaireProject.Shared
{
    public class SalariesDto
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? TelephoneFixe { get; set; }
        public string? TelephonePortable { get; set; }
        public string Service { get; set; }
        public string Site { get; set; }
    }
}