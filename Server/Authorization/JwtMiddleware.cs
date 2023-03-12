using BlazorAnnuaireProject.Authorization;
using BlazorAnnuaireProject.Helpers;
using BlazorAnnuaireProject.AnnuaireServices;
using Microsoft.Extensions.Options;

namespace BlazorAnnuaireProject.Authorization;
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
        if (context.Request.Path.Value.Contains("RefreshToken"))
        {
            await _next(context);
            return;
        }
        else if (context.Request.Path.Value.Contains("Login"))
        {
            await _next(context);
            return;
        }
        else if (context.Request.Path.Value.Contains("Register"))
        {
            await _next(context);
            return;
        }
        else
        {

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var AccessToken = jwtUtils.ValidateToken(token);
                if (AccessToken != null)
                {
                    // attach user to context on successful jwt validation
                    // context.Items["User"] = AdminService.GetUserByRefreshToken(AccessToken);
                }
            }

            await _next(context);
        }
    }
}