using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RoleId { get; set; }
        public Role Role { get; set; }

        [JsonIgnore] public string PasswordHash { get; set; }
        [JsonIgnore] public RefreshToken RefreshToken { get; set; }

    }
}