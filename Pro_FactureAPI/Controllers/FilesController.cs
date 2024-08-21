using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Pro_FactureAPI.Data;
using Pro_FactureAPI.Models;

namespace Pro_FactureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ProfactureDb _context;

        public FilesController(ProfactureDb context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("UploadFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFiles([FromQuery] Guid repertoireId, [FromForm] List<IFormFile> files, CancellationToken cancellationToken)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("Aucun fichier à télécharger.");
            }

            // Vérifier si le répertoire existe
            var repertoire = await _context.Repertoires.FindAsync(repertoireId);
            if (repertoire == null)
            {
                return NotFound("Répertoire non trouvé.");
            }

            var uploadResults = new List<Fichier>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue; // Ignorer les fichiers vides
                }

                var fileId = Guid.NewGuid();
                var originalFileName = file.FileName;
                var filename = await WriteFile(file, fileId);

                // Vérification supplémentaire que le nom de fichier n'est pas vide
                if (string.IsNullOrEmpty(filename))
                {
                    return BadRequest("Une erreur est survenue lors de l'enregistrement du fichier.");
                }

                var uploadDate = DateTime.Now;

                string fileType = file.ContentType switch
                {
                    "image/png" => "image",
                    "application/pdf" => "pdf",
                    _ => "autre"
                };

                var fichier = new Fichier
                {
                    IdFichier = fileId,
                    NomFichier = originalFileName,  // Utiliser le nom original du fichier
                    Type = fileType,
                    DateImportation = uploadDate,
                    RepertoireFk = repertoireId // Associer le fichier au répertoire
                };

                _context.Fichiers.Add(fichier);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Inclure des informations sur le répertoire si nécessaire
            var resultFiles = files.Select(file => new
            {
                IdFichier = Guid.NewGuid(), // Assurez-vous que vous gérez les ID correctement
                NomFichier = file.FileName,
                DateImportation = DateTime.Now,
                Type = file.ContentType switch
                {
                    "image/png" => "image",
                    "application/pdf" => "pdf",
                    _ => "autre"
                },
                RepertoireFk = repertoireId, // Inclure l'ID du répertoire
                Repertoire = new
                {
                    IdRepertoire = repertoire.IdRepertoire,
                    NomRepertoire = repertoire.NomRepertoire
                }
            }).ToList();

            return Ok(resultFiles);
        }



        private async Task<string> WriteFile(IFormFile file, Guid fileId)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.').Last(); // Obtenir l'extension du fichier
                filename = fileId.ToString() + extension; // Utiliser l'ID unique pour nommer le fichier

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception (ex: journaliser l'erreur)
                // Par exemple: Console.WriteLine(ex.Message);
            }
            return filename; // Retourner le nom du fichier généré
        }



        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
        [HttpDelete]
        [Route("DeleteFile/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteFile(Guid id)
        {
            var fichier = _context.Fichiers.Find(id);
            if (fichier == null)
            {
                return NotFound("Fichier non trouvé.");
            }

            _context.Fichiers.Remove(fichier);
            _context.SaveChanges();

            // Supprimer le fichier physique du disque
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", fichier.NomFichier);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            return NoContent();
        }

        [HttpPut]
        [Route("UpdateFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateFile([FromBody] Fichier updatedFichier)
        {
            var existingFichier = _context.Fichiers.Find(updatedFichier.IdFichier);
            if (existingFichier == null)
            {
                return NotFound("Fichier non trouvé.");
            }

            _context.Entry(existingFichier).CurrentValues.SetValues(updatedFichier);
            _context.SaveChanges();

            return Ok(existingFichier);
        }
    }
}