using BlazorAnnuaireProject.Entities;


public class Modification
{
    public int id { get; set; }

    public string description { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    public Admin Admin { get; set; }

}