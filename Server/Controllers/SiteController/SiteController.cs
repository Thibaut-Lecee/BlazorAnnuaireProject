using System.ComponentModel;
using BlazorAnnuaireProject.Context;
using BlazorAnnuaireProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAnnuaireProject.Controllers
{

    [Route("[controller]")]
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

        /// <summary>
        /// Récupère la liste des services d'un site.
        /// </summary>

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Description("Retourne une liste de tous les sites")]

        public async Task<IActionResult> GetAllSites()
        {
            try {
            var sites = await _siteService.GetAllSites();
            if (sites == null)
                return NotFound("Aucun site n'a été trouvé.");

            return Ok(sites);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Récupère la liste des salaries pour tous les sites.
        /// </summary>
        [HttpGet("WithSalaries")]
        [Description("Récupère tous les sites avec leurs salariés sous forme de liste")]
        public async Task<IActionResult> GetAllSitesWithSalaries()
        {
            try {
            var sites = await _siteService.GetAllSitesWithSalaries();
            if (sites == null)
                return NotFound("Aucun site n'a été trouvé.");
            return Ok(sites);
        } catch (Exception e) {
                return BadRequest(e.Message);
        }

            }

        [HttpGet("WithSalaries/{ville}")]
        [Description("Récupère un site par  la ville et on retourne les salariés sous forme de liste")]
        public async Task<IActionResult> GetSiteByNameAndSalaries(string ville)
        {

            try {
                  var site = await _siteService.GetSiteByNameAndSalaries(ville);
            if (site == null)
                return NotFound("Aucun site n'a été trouvé.");
            return Ok(site);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
          
        }

        [HttpGet("WithServices/{ville}")]
        [Description("Récupère un site par la ville et on retourne les services sous forme de liste")]
        public async Task<IActionResult> GetSiteWithServices(string ville)
        {
            try {
            var site = await _siteService.GetSiteByNameAndServices(ville);
            if (site == null)
                return NotFound("Aucun site n'a été trouvé.");
            return Ok(site);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }


        [HttpGet("WithSalariesAndServices/{ville}")]
        [Description("Récupère un site par la ville et on retourne les salariés et les services sous forme de liste")]
        public async Task<IActionResult> GetSiteByNameAndSalariesAndServices(string ville)
        {
            try {
            var site = await _siteService.GetSiteByNameAndSalariesAndServices(ville);
            if (site == null)
                return NotFound("Aucun site n'a été trouvé.");
            return Ok(site);
                
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{ville}")]
        [Description("Récupère un site par la ville")]
        public async Task<IActionResult> GetSiteByName(string ville)
        { 
            try {
 var site = await _siteService.GetSiteByName(ville);
            return Ok(site);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
           
        }

        [HttpPost]
        [Description("Créer un site")]
        public IActionResult CreateSite([FromBody] CreateSiteRequest site)
        {
            try {
            var newSite = _siteService.CreateSite(site);
            return StatusCode(201, new { message = "Site created", newSite });
            } catch (Exception e) {
                return StatusCode(500, new { message = "Erreur lors de la création du site", e });
            }
        }


        [HttpPut("{siteId}")]
        [Description("Mettre à jour un site par la ville")]
        public IActionResult UpdateSite([FromBody] UpdateRequest site, int siteId)
        {
            try {
            Console.WriteLine("siteId : " + site);
            var newSite = _siteService.UpdateSite(site, siteId);
            return StatusCode(201, new { message = "Site updated", newSite });
            } catch (Exception e) {
                return StatusCode(500, new { message = "Erreur lors de la mise à jour du site", e });
            }
        }


        [HttpDelete("{id}")]
        [Description("Supprimer un site par la ville")]
        public IActionResult DeleteSite(int id)
        {
             try {
            var removedSite = _siteService.DeleteSite(id);
            return Ok(new { message = "Site deleted", removedSite });

            } catch (Exception e) {
                return StatusCode(500, new { message = "Erreur lors de la mise à jour du site", e });
            }
        }


    }

}