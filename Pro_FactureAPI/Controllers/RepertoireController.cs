using Microsoft.AspNetCore.Mvc;
using Pro_FactureAPI.Models;
using Pro_FactureAPI.Service.Repertoire;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pro_FactureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepertoireController : ControllerBase
    {
        private readonly IRepertoire _repertoireService;

        public RepertoireController(IRepertoire repertoireService)
        {
            _repertoireService = repertoireService;
        }

        // GET: api/Repertoire
        [HttpGet]
        public ActionResult<IEnumerable<Repertoire>> GetRepertoires()
        {
            var repertoires = _repertoireService.GetAll();
            return Ok(repertoires);
        }

        // GET: api/Repertoire/{id}
        [HttpGet("{id}")]
        public ActionResult<Repertoire> GetRepertoire(Guid id)
        {
            var repertoire = _repertoireService.Get(id);
            if (repertoire == null)
            {
                return NotFound();
            }
            return Ok(repertoire);
        }
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Repertoire>> GetRepertoiresByUserId(Guid userId)
        {
            var repertoires = _repertoireService.GetRepertoiresByUserId(userId);
            if (repertoires == null || !repertoires.Any())
            {
                return NotFound("Aucun répertoire trouvé pour cet utilisateur.");
            }
            return Ok(repertoires);
        }

        // POST: api/Repertoire
        [HttpPost]
        public ActionResult<Repertoire> PostRepertoire(Repertoire repertoire)
        {
            var createdRepertoire = _repertoireService.Add(repertoire);
            return CreatedAtAction(nameof(GetRepertoire), new { id = createdRepertoire.IdRepertoire }, createdRepertoire);
        }
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] string nouveauNom)
        {
            if (string.IsNullOrWhiteSpace(nouveauNom))
            {
                return BadRequest("Le nom du répertoire ne peut pas être vide.");
            }

            if (_repertoireService.Update(id, nouveauNom))
            {
                return NoContent(); // Réponse sans contenu
            }
            return NotFound(); // Répertoire non trouvé
        }

        // DELETE: api/Repertoire/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRepertoire(Guid id)
        {
            var repertoire = _repertoireService.Get(id);
            if (repertoire == null)
            {
                return NotFound();
            }

            _repertoireService.Remove(id);
            return NoContent();
        }
    }
}
