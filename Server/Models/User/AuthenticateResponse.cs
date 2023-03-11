using System.Text.Json.Serialization;
using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpires { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpires { get; set; }
        public Role Role { get; set; }


        public AuthenticateResponse(Admin admin, string accessToken, string refreshToken, DateTime accessTokenExpires, DateTime refreshTokenExpires)
        {
            Id = admin.AdminId;
            Email = admin.Email;
            AccessToken = accessToken;
            AccessTokenExpires = accessTokenExpires;
            RefreshToken = refreshToken;
            RefreshTokenExpires = refreshTokenExpires;
            Role = admin.Role;
    
        }
    }
}