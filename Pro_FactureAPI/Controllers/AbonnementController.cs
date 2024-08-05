using Microsoft.AspNetCore.Mvc;
using Pro_FactureAPI.Models;
using Pro_FactureAPI.Service.Abonnement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pro_FactureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonnementsController : ControllerBase
    {
        private readonly IAbonnement _abonnementService;

        public AbonnementsController(IAbonnement abonnementService)
        {
            _abonnementService = abonnementService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Abonnement>> GetAbonnements()
        {
            var abonnements = _abonnementService.GetAll();
            return Ok(abonnements);
        }

        [HttpGet("{id}")]
        public ActionResult<Abonnement> GetAbonnement(Guid id)
        {
            var abonnement = _abonnementService.Get(id);
            if (abonnement == null)
            {
                return NotFound();
            }
            return Ok(abonnement);
        }

        [HttpPost]
        public ActionResult<Abonnement> PostAbonnement(Abonnement abonnement)
        {
            var newAbonnement = _abonnementService.Add(abonnement);
            return CreatedAtAction(nameof(GetAbonnement), new { id = newAbonnement.IdAbonnement }, newAbonnement);
        }

        [HttpPut("{id}")]
        public IActionResult PutAbonnement(Guid id, Abonnement abonnement)
        {
            if (id != abonnement.IdAbonnement)
            {
                return BadRequest();
            }

            var result = _abonnementService.Update(abonnement);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAbonnement(Guid id)
        {
            var abonnement = _abonnementService.Get(id);
            if (abonnement == null)
            {
                return NotFound();
            }

            _abonnementService.Remove(id);
            return NoContent();
        }
        [HttpPatch("RendreInactif/{id}")]
        public IActionResult RendreInactif(Guid id)
        {
            var abonnement = _abonnementService.Get(id);
            if (abonnement == null)
            {
                return NotFound();
            }

            var updated = _abonnementService.SetInactive(id);
            if (!updated)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Échec de la mise à jour de l'abonnement.");
            }

            return NoContent();
        }
    
    }
}