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
        public ActionResult<Repertoire> GetRepertoire(int id)
        {
            var repertoire = _repertoireService.Get(id);
            if (repertoire == null)
            {
                return NotFound();
            }
            return Ok(repertoire);
        }

        // POST: api/Repertoire
        [HttpPost]
        public ActionResult<Repertoire> PostRepertoire(Repertoire repertoire)
        {
            var createdRepertoire = _repertoireService.Add(repertoire);
            return CreatedAtAction(nameof(GetRepertoire), new { id = createdRepertoire.IdRepertoire }, createdRepertoire);
        }

        // PUT: api/Repertoire/{id}
        [HttpPut("{id}")]
        public IActionResult PutRepertoire(int id, Repertoire repertoire)
        {
            if (id != repertoire.IdRepertoire)
            {
                return BadRequest();
            }

            var updated = _repertoireService.Update(repertoire);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Repertoire/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRepertoire(int id)
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
