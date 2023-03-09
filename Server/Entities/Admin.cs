using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Entities
{
    public class Admin
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public string VerifyToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Address { get; set; }

        public DateTime VerifiedAt { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }


        [JsonIgnore] public string PasswordHash { get; set; }
        [JsonIgnore] public RefreshToken RefreshToken { get; set; }

    }
}