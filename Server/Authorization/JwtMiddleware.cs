using BlazorAnnuaireProject.AnnuaireServices.AdminService;
using BlazorAnnuaireProject.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAnnuaireProject.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAdminService adminService, IJwtUtils jwtUtils)
        {
            if (context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method == HttpMethods.Delete)
            {
                
                // Ici si le path contient Logout ou Login on ne vérifie pas l'AccessToken qui est obligatoire pour les requetes put, post et delete et certains get
                if (context.Request.Path.Value.Contains("Logout") || context.Request.Path.Value.Contains("Login"))
                {
                    
                    await _next(context);
                    return;
                }

                // On récupère le token dans le header
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                Console.WriteLine("JwtMiddleware: " + token);
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                // Vérifier si le token est valide
                var accessToken = jwtUtils.ValidateToken(token);
                if (accessToken == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                // Vérifier si l'utilisateur est un administrateur valide
                var admin = adminService.GetAdminByRefreshToken(accessToken);
                if (admin == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                // Ajouter l'utilisateur à la requête
                context.Items["Admin"] = admin;
            }

            // On continue la requête
            await _next(context);
        }
    }
}
