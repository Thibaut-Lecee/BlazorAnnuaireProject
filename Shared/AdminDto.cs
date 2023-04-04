using System.ComponentModel.DataAnnotations;
namespace BlazorAnnuairePro.Shared
{
    public class AdminDto
    {
          [Required, EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
    }
}