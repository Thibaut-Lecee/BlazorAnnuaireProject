using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Shared.Entities;


public class RefreshToken
{
    [Key]
    // Ignore la propriété Id lors de la sérialisation
    [JsonIgnore]
    public int Id { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpires { get; set; }

    public string NewToken { get; set; }
    public DateTime NewTokenExpires { get; set; }
    public DateTime Created { get; set; }

    public bool IsTokenExpired => DateTime.UtcNow >= AccessTokenExpires;
    public bool IsNewTokenExpired => DateTime.UtcNow >= NewTokenExpires;
    public bool IsActive => !IsNewTokenExpired;
    public Admin Admin { get; set; }
    public int AdminId { get; set; }


} // End Class RefreshToken