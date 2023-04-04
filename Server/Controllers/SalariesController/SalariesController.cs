
using Microsoft.AspNetCore.Mvc;
using BlazorAnnuaireProject.AnnuaireServices.SalarieService;
using BlazorAnnuaireProject.Context;

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

        [HttpGet]
        public async Task<IActionResult> GetAllSalaries()
        {
            var salaries = await _salarieService.GetAllSalaries();
            if (salaries == null)
            {
                return NotFound();
            }
            return Ok(salaries);
        }



        [HttpGet("SalarieByEmail/{email}")]
        public async Task<IActionResult> GetSalarieByEmail(string email)
        {
            var salarie = await _salarieService.GetSalariesByEmail(email);
            return Ok(salarie);
        }
        [HttpGet("SalarieByEmailWithAssociations/{email}")]
        public async Task<IActionResult> GetSalarieByEmaildWithAssociations(string email)
        {
            var salarie = await _salarieService.GetSalariesByEmailWithAssociations(email);
            return Ok(salarie);
        }
    }

}

