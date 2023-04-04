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
                // Permettre RefreshToken même si le token n'est pas valide
                if (context.Request.Path.Value.Contains("RefreshToken") || context.Request.Path.Value.Contains("Login"))
                {
                    await _next(context);
                    return;
                }

                // Vérifier si la requête doit être authentifiée
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                Console.WriteLine("JwtMiddleware: " + token);
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

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

            await _next(context);
        }
    }
}
