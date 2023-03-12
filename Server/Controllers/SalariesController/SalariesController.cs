
using BlazorAnnuaireProject.Entities;
using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.AnnuaireServices.SalarieService;
using BlazorAnnuaireProject.Context;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class SalariesController : ControllerBase
    {
        private readonly ISalarieService _salarieService;

        private readonly DataContext _context;
        public SalariesController(ISalarieService salarieService, DataContext context)
        {
            _salarieService = salarieService;
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllSalaries()
        {
            var salaries = await _salarieService.GetAllSalaries();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var json = JsonSerializer.Serialize(salaries, options);
            return Ok(json);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalarieById(int id)
        {
            var salarie = await _salarieService.GetSalariesById(id);
            return Ok(salarie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSalaries(Salaries salarie)
        {
            var newSalarie = await _salarieService.CreateSalaries(salarie);
            return Ok(newSalarie);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSalaries(Salaries salarie)
        {
            await _salarieService.UpdateSalaries(salarie);
            return Ok("Salarie updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalaries(int id)
        {
            await _salarieService.DeleteSalaries(id);
            return Ok("Salarie deleted");
        }


    }
}

