namespace BlazorAnnuaireProject.Helpers;


// Permet de passer les données du fichier appsettings.json à l'application
public class AppSettings
{
    public string Secret { get; set; }
    
    public string PasswordAdmin { get; set; }

    public string EmailAdmin { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}