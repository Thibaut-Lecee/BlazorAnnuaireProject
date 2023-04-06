using System.Text.Json.Serialization;
using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Shared
{
    public class AuthenticateResponse
    {
        private Admin admin;
        private string name;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("accessTokenExpires")]
        public DateTime AccessTokenExpires { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("refreshTokenExpires")]
        public DateTime RefreshTokenExpires { get; set; }

        [JsonPropertyName("role")]
        public Role Role { get; set; }

        [JsonConstructor]
        public AuthenticateResponse(int id, string email, string accessToken, DateTime accessTokenExpires, string refreshToken, DateTime refreshTokenExpires, Role role)
        {
            Id = id;
            Email = email;
            AccessToken = accessToken;
            AccessTokenExpires = accessTokenExpires;
            RefreshToken = refreshToken;
            RefreshTokenExpires = refreshTokenExpires;
            Role = role;
        }

        public AuthenticateResponse(Admin admin, string accessToken, string refreshToken, DateTime accessTokenExpires, DateTime refreshTokenExpires, Role role)
        {
            this.admin = admin;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpires = accessTokenExpires;
            RefreshTokenExpires = refreshTokenExpires;
            Role = role;
        }

    }
}
