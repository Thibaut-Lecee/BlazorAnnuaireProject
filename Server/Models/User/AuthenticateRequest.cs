using System.ComponentModel.DataAnnotations;
namespace BlazorAnnuaireProject.Models;


public class AuthenticateRequest
{
    [Required, EmailAddress] public string Email { get; set; }
    [Required, DataType(DataType.Password)] public string Password { get; set; }
}