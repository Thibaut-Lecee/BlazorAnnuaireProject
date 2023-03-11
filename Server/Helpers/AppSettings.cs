namespace BlazorAnnuaireProject.Helpers;

public class AppSettings
{
    public string Secret { get; set; }
    
    public string PasswordAdmin { get; set; }

    public string EmailAdmin { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}