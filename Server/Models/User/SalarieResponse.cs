using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Models
{
    public class CreateSalarieReponse
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string TelephoneFixe { get; set; }
        public string TelephonePortable { get; set; }
        public string? Service { get; set; }
        public string? Site { get; set; }

        public CreateSalarieReponse(Salaries salarie)
        {
            if (salarie != null)
            {
                Id = salarie.Id;
                Nom = salarie.Nom;
                Prenom = salarie.Prenom;
                Email = salarie.Email;
                TelephoneFixe = salarie.TelephoneFixe;
                TelephonePortable = salarie.TelephonePortable;
                Service = salarie.Service?.Nom;
                Site = salarie.Site?.Ville;
            }
        }
    }


    public class UpdateSalarieResponse
    {
        public string? Nom { get; set; }

        public string? Prenom { get; set; }

        public string? Email { get; set; }

        public string? TelephoneFixe { get; set; }

        public string? TelephonePortable { get; set; }

        public string? Service { get; set; }

        public string? Site { get; set; }

        public UpdateSalarieResponse(Salaries salarie)
        {
            Nom = salarie?.Nom;
            Prenom = salarie?.Prenom;
            Email = salarie?.Email;
            TelephoneFixe = salarie?.TelephoneFixe;
            TelephonePortable = salarie?.TelephonePortable;
            Service = salarie?.Service?.Nom;
            Site = salarie?.Site?.Ville;
        }
    }
}