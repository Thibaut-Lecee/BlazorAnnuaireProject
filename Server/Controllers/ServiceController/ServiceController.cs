using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Models;
using System.ComponentModel;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("[controller]")]
    [ApiController]

    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly DataContext _context;

        public ServiceController(IServiceService serviceService, DataContext context)
        {
            _serviceService = serviceService;
            _context = context;
        }

        
        [HttpGet]
        [Description("Récupère tous les services sous forme de liste")]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServices();

            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByVille(int id)
        {
            var service = await _serviceService.GetServiceById(id);
            return Ok(service);
        }

        [HttpPost]
        public IActionResult CreateService([FromBody] CreateServiceRequest service)
        {
            var newService = _serviceService.CreateService(service);
            return StatusCode(201, new { message = "Service created", newService });
        }

        [HttpPost("{ville}")]
        public IActionResult CreateServiceOnSite([FromBody] CreateServiceRequest service, string ville)
        {
            var newService = _serviceService.CreateServiceOnSite(service, ville);
            return StatusCode(201, new { message = "Service created", newService });
        }


        [HttpPut("{serviceId}")]
        public IActionResult UpdateService([FromBody] UpdateServiceRequest service, int serviceId)
        {
            var updateService = _serviceService.UpdateService(service, serviceId);
            return StatusCode(201, new { message = "Service updated", updateService });
        }


        [HttpDelete("{service}")]
        public IActionResult DeleteService(string service)
        {
            var removedSite = _serviceService.DeleteService(service);
            return StatusCode(201, new { message = "Service deleted", removedSite });
        }
    }
}