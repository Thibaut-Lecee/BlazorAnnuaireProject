namespace BlazorAnnuaireProject.Models
{
    public class RegisterRequest
    {
       public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    
    public string TelephoneFixe { get; set; }
    public string TelephonePortable { get; set; }

    public int ServiceId { get; set; }
    public int SiteId { get; set; }

    //1ère association avec le Service  (1 salarié = 1 service)
    }
}