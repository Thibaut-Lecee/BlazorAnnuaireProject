using BlazorAnnuaireProject.Models;
using BlazorAnnuaireProject.Shared.Entities;

public interface IServiceService
{
    Task<List<Services>> GetAllServices();
    Task<Services> GetServiceById(int id);


    CreateServiceResponse CreateService(CreateServiceRequest service);

    CreateServiceResponse CreateServiceOnSite(CreateServiceRequest service, string Ville);
    UpdateServiceResponse UpdateService(UpdateServiceRequest service, int serviceId);

    DeleteServiceResponse DeleteService(string nom);
}