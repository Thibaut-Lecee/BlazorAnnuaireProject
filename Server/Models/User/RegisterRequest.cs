using System.ComponentModel.DataAnnotations;

namespace BlazorAnnuaireProject.Models
{
    public class RegisterRequestSalarie
    {
        [Required] public string Nom { get; set; }
        [Required] public string Prenom { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        [Required] public string TelephoneFixe { get; set; }
        [Required] public string TelephonePortable { get; set; }



        //1ère association avec le Service  (1 salarié = 1 service)

    }
    public class UpdateSalarieRequest
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }

        public string? TelephoneFixe { get; set; }
        public string? TelephonePortable { get; set; }

        public string? Service { get; set; }

        public string? Site { get; set; }
    }

}