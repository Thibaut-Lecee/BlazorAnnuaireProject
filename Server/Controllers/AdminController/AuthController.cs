using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;
using Microsoft.Extensions.Options;
using BlazorAnnuaireProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.Authorization;
using BlazorAnnuaireProject.Context;
using Microsoft.AspNetCore.Authorization;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private readonly DataContext _context;

        public AuthController(IAdminService adminService, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings, DataContext context)
        {
            _adminService = adminService;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            _context = context;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthenticateRequest model)
        {
            var response = _adminService.Login(model);

            if (response.RefreshToken != null)
            {
                Console.WriteLine("RefreshToken: " + response.AccessToken);
                SetTokenCookie(response.AccessToken, response.Id, response.RefreshToken, response.AccessTokenExpires, response.RefreshTokenExpires);
            }
            return StatusCode(200, response);
        }

        [HttpPost("Register")]

        public IActionResult Register([FromBody] RegisterRequestSalarie request)
        {
            try
            {
                var created = _adminService.CreateSalarie(request);
                return StatusCode(201, created);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.HttpStatusCode, ex.Message);
            }

        }

        [HttpPost("RegisterOnService/{Service}")]
        public IActionResult RegisterOnService([FromBody] RegisterRequestSalarie request, string Service)
        {
            var created = _adminService.CreateSalarieOnService(request, Service);
            return StatusCode(201, created);
        }

        [HttpPost("RegisterOnServiceAndSite/{Site}/{Service}")]
        public IActionResult RegisterOnServiceAndSite([FromBody] RegisterRequestSalarie request, string Site, string Service)
        {
            try
            {
                var created = _adminService.CreateSalarieOnServiceAndSite(request, Site, Service);
                return StatusCode(201, created);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPut("UpdateSalarie")]
        public IActionResult UpdateSalarie([FromBody] UpdateSalarieRequest request)
        {
            try
            {
                var created = _adminService.UpdateSalarie(request);
                return StatusCode(201, created);
            }
             catch (AppException ex)
            {
                return StatusCode(ex.HttpStatusCode, ex.Message);
            }
        
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var admin = _adminService.GetAdminByRefreshToken(refreshToken);
            var newAccessToken = _jwtUtils.GenerateAccessToken(admin);
            SetTokenCookie(newAccessToken.AccessToken, admin.RoleId, newAccessToken.NewToken, newAccessToken.AccessTokenExpires, newAccessToken.NewTokenExpires);
            return StatusCode(200, newAccessToken);
        }

        [HttpDelete("DeleteSalarie/{email}")]
        public IActionResult DeleteSalarie(string email)
        {
            _adminService.DeleteSalarie(email);
            return StatusCode(200, "Salarie deleted");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var admin = _adminService.GetAdminByRefreshToken(refreshToken);
            admin.RefreshToken = null;
            _context.SaveChanges();
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("AccessToken");
            return StatusCode(200);
        }

        [HttpGet("GetAdmin")]
        [Authorize]
        public IActionResult GetAdmin()
        {
            var token = Request.Cookies["AccessToken"];
            var admin = CheckToken(token);
            return StatusCode(200, admin);
        }



        private void SetTokenCookie(string token, int id, string refreshToken, DateTime tokenExpires, DateTime newTokenExpires)
        {
            // append cookie with refresh token to the http response

            var cookieOptionsRefresh = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                Expires = newTokenExpires
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptionsRefresh);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                Expires = tokenExpires
            };
            Response.Cookies.Append("AccessToken", token, cookieOptions);

        }

        private Admin CheckToken(string token)
        {

            if (token != null)
            {
                Console.WriteLine("token : " + token);
                var admin = _adminService.GetAdminByRefreshToken(token);
                if (admin.RefreshToken.AccessToken != null && admin.RefreshToken.AccessTokenExpires > DateTime.UtcNow)
                {
                    var timeLeft = (int)(admin.RefreshToken.AccessTokenExpires - DateTime.UtcNow).TotalSeconds;
                    var expirationTime = DateTimeOffset.UtcNow.AddSeconds(timeLeft);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = expirationTime
                    };
                    Response.Cookies.Append("AccessToken", admin.RefreshToken.AccessToken, cookieOptions);
                    return admin;
                }
                else if (admin.RefreshToken.AccessToken != null && admin.RefreshToken.AccessTokenExpires < DateTime.UtcNow && admin.RefreshToken.NewTokenExpires > DateTime.UtcNow && admin.RefreshToken.NewToken != null)
                {
                    // generate new AccessToken for user 
                    var newAccessToken = _jwtUtils.GenerateAccessToken(admin);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };
                    // update user with new AccessToken
                    admin.RefreshToken.AccessTokenExpires = DateTime.UtcNow.AddHours(1);
                    admin.RefreshToken.AccessToken = newAccessToken.AccessToken;
                    _context.Update(admin);
                    _context.SaveChanges();
                    // append cookie with new AccessToken to the http response

                    Response.Cookies.Append("AccessToken", newAccessToken.AccessToken, cookieOptions);
                    return admin;
                }
                else
                {
                    throw new UnauthorizedAccessException("Access token not found or expired.");
                }

            }
            throw new UnauthorizedAccessException("Invalid access token.");

        }

    }



}