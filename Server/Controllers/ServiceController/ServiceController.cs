using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Models;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("api/[controller]")]
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

        // [HttpPost("{ville}")]
        // public IActionResult CreateServiceOnSite([FromBody] CreateServiceRequest service, string ville)
        // {
        //     var newService = _serviceService.CreateServiceOnSite(service, ville);
        //     return StatusCode(201, new { message = "Service created", newService });
        // }


        [HttpPut("{nom}")]
        public IActionResult UpdateService([FromBody] UpdateServiceRequest service, string nom)
        {
            var updateService = _serviceService.UpdateService(service, nom);
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