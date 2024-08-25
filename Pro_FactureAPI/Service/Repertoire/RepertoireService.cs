
namespace Pro_FactureAPI.Service.Repertoire;
using Pro_FactureAPI.Data;
using Pro_FactureAPI.Models;
using Microsoft.EntityFrameworkCore;

public class RepertoireService : IRepertoire
    {
        private readonly ProfactureDb _context;

        public RepertoireService(ProfactureDb context)
        {
            _context = context;
        }

    public IEnumerable<Repertoire> GetAll()
    {
        return _context.Repertoires
            .Include(r => r.Fichiers)
            .ToList();
    }

    public Repertoire Get(Guid id)
        {
        return _context.Repertoires
       .Include(r => r.Fichiers)
       .FirstOrDefault(r => r.IdRepertoire == id);
    }

        public Repertoire Add(Repertoire repertoire)
        {
            _context.Repertoires.Add(repertoire);
            _context.SaveChanges();
            return repertoire;
        }

        public void Remove(Guid id)
        {
            var repertoire = _context.Repertoires.Find(id);
            if (repertoire != null)
            {
                _context.Repertoires.Remove(repertoire);
                _context.SaveChanges();
            }
        }

    public bool Update(Guid id, string nouveauNom)
    {
        var existingItem = _context.Repertoires.Find(id);
        if (existingItem == null)
        {
            return false; // Répertoire non trouvé
        }

        existingItem.NomRepertoire = nouveauNom; // Met à jour uniquement le nom du répertoire

        _context.SaveChanges();
        return true;
    }
    public IEnumerable<Repertoire> GetRepertoiresByUserId(Guid userId)
    {
        return _context.Repertoires
            .Include(r => r.Fichiers)
            .Where(r => r.UtilisateurId == userId) // Filtrer par userId
            .ToList();
    }
}


