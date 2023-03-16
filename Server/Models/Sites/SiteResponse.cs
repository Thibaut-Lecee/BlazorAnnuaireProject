using BlazorAnnuaireProject.Entities;

namespace BlazorAnnuaireProject.Models;


public class CreateSiteResponse
{

    public int Id { get; set; }
    public string Ville { get; set; }

    public string Description { get; set; }

    public CreateSiteResponse(Site site)
    {
        Id = site.Id;
        Ville = site.Ville;
        Description = site.Description;
    }

}

public class UpdateResponse
{
    public int Id { get; set; }
    public string Ville { get; set; }

    public string Description { get; set; }

    public string Message { get; set; }

    public UpdateResponse(Site site, string message = "")
    {
        Id = site.Id;
        Ville = site.Ville;
        Description = site.Description;
        Message = message;
    }

}

public class DeleteResponse
{
    private string infoRemoved;
    private string v;

    public DeleteResponse(string infoRemoved, string v)
    {
        this.infoRemoved = infoRemoved;
        this.v = v;
    }
}