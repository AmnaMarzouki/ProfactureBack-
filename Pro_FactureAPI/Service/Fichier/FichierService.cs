


namespace Pro_FactureAPI.Service.Fichier;
      using Pro_FactureAPI.Data;
using Pro_FactureAPI.Models;

    public class FichierService : IFichier
    {
        private readonly ProfactureDb _context;

        public FichierService(ProfactureDb context)
        {
            _context = context;
        }


        public IEnumerable<Fichier> GetAll()
        {
            return _context.Fichiers.ToList();
        }

        public Fichier Get(Guid id)
        {
            return _context.Fichiers.Find(id);
        }

        public Fichier Add(Fichier fichier)
        {
            _context.Fichiers.Add(fichier);
            _context.SaveChanges();
            return fichier;
        }

        public void Remove( Guid id)
        {
            var fichier = _context.Fichiers.Find(id);
            if (fichier != null)
            {
                _context.Fichiers.Remove(fichier);
                _context.SaveChanges();
            }
        }

        public bool Update(Fichier item)
        {
            var existingItem = _context.Fichiers.Find(item.IdFichier);
            if (existingItem == null)
            {
                return false;
            }

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            _context.SaveChanges();
            return true;
        }
    }

