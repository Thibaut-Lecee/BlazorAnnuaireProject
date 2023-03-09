using BlazorAnnuaireProject.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class Role
 {      
        [Key]
        [JsonIgnore] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  ICollection<Admin> Admins { get; set; }
}