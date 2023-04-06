using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Shared;

public class SiteAndService
{
    public int SiteId { get; set; }
    public int ServiceId { get; set; }

    public Site Site { get; set; }
    public Services Service { get; set; }
}