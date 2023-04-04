using BlazorAnnuaireProject.Shared;
using BlazorAnnuaireProject.Shared.Entities;

namespace BlazorAnnuaireProject.Models;


public class CreateServiceResponse
{
    public int Id { get; set; }
    public string Nom { get; set; }

    public CreateServiceResponse(Services service)
    {
        Id = service.Id;
        Nom = service.Nom;

    }

}

public class UpdateServiceResponse
{

    public int Id { get; set; }
    public string Nom { get; set; }

    public string Message { get; set; }

    public UpdateServiceResponse(Services service, string message = "")
    {
        Id = service.Id;
        Nom = service.Nom;
        Message = message;
    }
}

public class DeleteServiceResponse
{
    private string infoRemoved;
    private string v;

    public DeleteServiceResponse(string infoRemoved, string v)
    {
        this.infoRemoved = infoRemoved;
        this.v = v;
    }
}