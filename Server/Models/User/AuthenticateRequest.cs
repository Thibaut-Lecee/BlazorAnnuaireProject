using System.ComponentModel.DataAnnotations;
namespace BlazorAnnuaireProject.Models;


public class AuthenticateRequest
{
    [Required] public string Email { get; set; }
   [Required] public string Password { get; set; }
}