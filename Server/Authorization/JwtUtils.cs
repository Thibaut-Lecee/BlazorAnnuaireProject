using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Shared;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Authorization;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IJwtUtils
{
    public RefreshToken GenerateAccessToken(Admin admin);
    public string ValidateToken(string token);
    public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    public RefreshToken GenerateRefreshToken(Admin admin);
}

public class JwtUtils : IJwtUtils
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;


    public JwtUtils(IOptions<AppSettings> appSettings, DataContext context)
    {
        _context = context;
        _appSettings = appSettings.Value;

    }

    // Génère un token d'accès et un token de rafraîchissement et les stocke dans la base de données pour l'administrateur spécifié
    public RefreshToken GenerateAccessToken(Admin admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        // Récupérer le rôle de l'administrateur pour le stocker dans le token
        var Role = _context.Roles.Include(r => r.Id).SingleOrDefault(r => r.Id == admin.AdminId).Name;
        Console.WriteLine("Role de l'utilisateur " + Role);

        // Crée un token d'accès valide pendant 8 heures
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", admin.AdminId.ToString()), new Claim(ClaimTypes.Role, Role) }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = _context.RefreshToken.SingleOrDefault(r => r.AdminId == admin.AdminId);

        if (refreshToken != null && refreshToken.NewToken != null && refreshToken.NewTokenExpires > DateTime.UtcNow)
        {
            if (refreshToken.AccessTokenExpires < DateTime.UtcNow)
            {
                // Mettre à jour le token d'accès expiré avec un nouveau token d'accès valide
                accessToken = tokenHandler.CreateToken(tokenDescriptor);
                refreshToken.AccessToken = tokenHandler.WriteToken(accessToken);
                refreshToken.AccessTokenExpires = tokenDescriptor.Expires ?? throw new Exception("Could not create access token");
                _context.SaveChanges();
            }
            Console.WriteLine("refreshToken.AccessToken: " + refreshToken.AccessToken);
            refreshToken.AccessTokenExpires = tokenDescriptor.Expires ?? throw new Exception("Could not create access token");
            return refreshToken;
        }


        // Crée un token de rafraîchissement valide pendant 7 jours

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", admin.AdminId.ToString()), new Claim(ClaimTypes.Role, Role) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var newRefreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

        // Stocke le refresh token dans la base de données pour une persistance de connexion
        var refreshTokenBdd = new RefreshToken
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            AccessTokenExpires = tokenDescriptor.Expires ?? throw new Exception("Could not create access token"),
            NewToken = tokenHandler.WriteToken(newRefreshToken),
            NewTokenExpires = refreshTokenDescriptor.Expires ?? throw new Exception("Could not create refresh token"),
            Created = DateTime.UtcNow,
            AdminId = admin.AdminId,
        };
        _context.RefreshToken.Add(refreshTokenBdd);

        _context.SaveChanges();

        // Retourne les informations de token pour une utilisation immédiate
        return new RefreshToken
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            AccessTokenExpires = tokenDescriptor.Expires ?? throw new Exception("Could not create access token"),
            NewToken = refreshTokenBdd.NewToken,
            NewTokenExpires = refreshTokenBdd.NewTokenExpires,
            Created = DateTime.UtcNow,
        };
    }



    public string ValidateToken(string token)
    {
        var tokenValidation = _context.RefreshToken.SingleOrDefault(r => r.AccessToken == token);

        // si le token n'est pas valide car il n'est pas dans la base de données ou qu'il est expiré on renvoie une exception
        // Il faudrait générer un code status 401 pour que le front puisse rediriger vers la page de connexion
        if (tokenValidation is null)
        {
            throw new Exception("Token invalide car c'est pas le bon");
        }


        var tokenHandler = new JwtSecurityTokenHandler();
        if (_appSettings.Secret is null)
        {
            throw new Exception("secret is null");
        }
        var key = string.IsNullOrEmpty(_appSettings?.Secret) ? throw new Exception("secret is null") : Encoding.ASCII.GetBytes(_appSettings.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var adminId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            var tokenBdd = _context.RefreshToken.SingleOrDefault(r => r.AdminId == adminId);

            // retroune le token d'accès si la validation est réussie
            Console.WriteLine("tokenBdd.AccessToken: " + tokenBdd.AccessToken);
            return tokenBdd.AccessToken;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }


    // Generation du token de rafraîchissement
    public RefreshToken GenerateRefreshToken(Admin admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        // Create claims for refresh token
        var claims = new List<Claim>
    {
        new Claim("id", admin.AdminId.ToString()),
        new Claim(ClaimTypes.Role, _context.Roles.Include(r => r.Id).SingleOrDefault(r => r.Id == admin.AdminId).Name)
    };
        var refreshToken = _context.RefreshToken.SingleOrDefault(r => r.AdminId == admin.AdminId);

        // Genere un nouveau token d'accès valide pendant 8 heures
        var refreshAccessToken = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // on crée le token
        var newRefreshTokenAccess = tokenHandler.CreateToken(refreshAccessToken);

        // on met à jour dans la base de donnée 
        var refreshTokenBdd = new RefreshToken
        {
            AccessToken = tokenHandler.WriteToken(newRefreshTokenAccess),
            AccessTokenExpires = refreshAccessToken.Expires ?? throw new Exception("Could not create refresh token"),
            Created = DateTime.UtcNow,
            NewToken = refreshToken.NewToken,
            NewTokenExpires = refreshToken.NewTokenExpires,
            AdminId = admin.AdminId
        };
        _context.RefreshToken.Update(refreshTokenBdd);
        return refreshTokenBdd;
    }


    // Vérifie si le token est valide et retourne le principal de l'utilisateur si c'est le cas 
    public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(signingKey);
    
        try
        {
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }

}
