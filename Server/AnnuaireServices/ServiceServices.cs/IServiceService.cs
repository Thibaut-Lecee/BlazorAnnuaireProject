using BlazorAnnuaireProject.Entities;

public interface IServiceService
{
    Task<List<Services>> GetAllServices();
    Task<Services> GetServiceById(int id);
    Task<int> CreateService(Services service);
    Task UpdateService(Services service);
    Task DeleteService(int id);
}