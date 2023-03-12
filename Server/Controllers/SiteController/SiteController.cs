

using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Entities;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSiteById(int id)
        {
            var site = await _siteService.GetSiteById(id);
            return Ok(site);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSite(Site site)
        {
            var newSite = await _siteService.CreateSite(site);
            return Ok(newSite);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSite(Site site)
        {
            await _siteService.UpdateSite(site);
            return Ok("Site updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            await _siteService.DeleteSite(id);
            return Ok("Site deleted");
        }


    }

}