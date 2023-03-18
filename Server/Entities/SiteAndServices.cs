namespace BlazorAnnuaireProject.Entities;

public class SiteAndService
{
    public int SiteId { get; set; }
    public int ServiceId { get; set; }

    public Site Site { get; set; }
    public Services Service { get; set; }
}