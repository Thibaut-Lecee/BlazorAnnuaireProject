
using BlazorAnnuaireProject.Entities;
using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.Context;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task<IActionResult> CreateService(Services service)
        {
            var newService = await _serviceService.CreateService(service);
            return Ok(newService);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateService(Services service)
        {
            await _serviceService.UpdateService(service);
            return Ok("Service updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _serviceService.DeleteService(id);
            return Ok("Service deleted");
        }
    }
}