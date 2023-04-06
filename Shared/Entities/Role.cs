using BlazorAnnuaireProject.Shared;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Shared.Entities;
public class Role
{
    [Key]
    [JsonIgnore] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Admin> Admins { get; set; }
}