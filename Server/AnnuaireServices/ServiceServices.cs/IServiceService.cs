using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;

public interface IServiceService
{
    Task<List<Services>> GetAllServices();
    Task<Services> GetServiceById(int id);

    CreateServiceResponse CreateService(CreateServiceRequest service);

    // CreateServiceResponse CreateServiceOnSite(CreateServiceRequest service, string Ville);
    UpdateServiceResponse UpdateService(UpdateServiceRequest service, string nom);

    DeleteServiceResponse DeleteService(string nom);
}