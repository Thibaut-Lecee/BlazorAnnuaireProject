using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class SiteController : ControllerBase
    {

        private readonly ISiteService _siteService;
        private readonly DataContext _context;

        public SiteController(ISiteService siteService, DataContext context)
        {
            _siteService = siteService;
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllSites()
        {
            var sites = await _siteService.GetAllSites();
            return Ok(sites);
        }

        [HttpGet("withSalaries")]
        public async Task<IActionResult> GetAllSitesWithSalaries()
        {
            var sites = await _siteService.GetAllSitesWithSalaries();
            return Ok(sites);
        }

        [HttpGet("withServices")]
        public async Task<IActionResult> GetAllSiteWithServices()
        {
            var sites = await _siteService.GetAllSiteWithServices();
            return Ok(sites);
        }

        [HttpGet("withSalariesAndServices")]
        public async Task<IActionResult> GetAllSiteWithSalariesAndServices()
        {
            var sites = await _siteService.GetAllSiteWithSalariesAndServices();
            return Ok(sites);
        }

        [HttpGet("withSalaries/{ville}")]
        public async Task<IActionResult> GetSiteByNameAndSalaries(string ville)
        {
            var site = await _siteService.GetSiteByNameAndSalaries(ville);
            return Ok(site);
        }

        [HttpGet("withServices/{ville}")]
        public async Task<IActionResult> GetSiteByNameAndServices(string ville)
        {
            var site = await _siteService.GetSiteByNameAndServices(ville);
            return Ok(site);
        }

        [HttpGet("withSalariesAndServices/{ville}")]
        public async Task<IActionResult> GetSiteByNameAndSalariesAndServices(string ville)
        {
            var site = await _siteService.GetSiteByNameAndSalariesAndServices(ville);

            return Ok(site);
        }


        [HttpGet("{ville}")]
        public async Task<IActionResult> GetSiteByName(string ville)
        {
            var site = await _siteService.GetSiteByName(ville);
            return Ok(site);
        }

        [HttpPost]
        public IActionResult CreateSite([FromBody] CreateSiteRequest site)
        {
            var newSite = _siteService.CreateSite(site);
            return StatusCode(201, new { message = "Site created", newSite });
        }


        [HttpPut("{ville}")]
        public IActionResult UpdateSite([FromBody] UpdateRequest site, string ville)
        {
            var newSite = _siteService.UpdateSite(site, ville);
            return StatusCode(201, new { message = "Site updated", newSite });
        }


        [HttpDelete("{ville}")]
        public IActionResult DeleteSite(string ville)
        {
            var removedSite = _siteService.DeleteSite(ville);
            return Ok(new { message = "Site deleted", removedSite });
        }


    }

}