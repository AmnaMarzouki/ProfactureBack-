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
                var filename = await WriteFile(file, fileId);
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
                    NomFichier = filename,
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
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
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
            }
            return filename;
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
    }
}
