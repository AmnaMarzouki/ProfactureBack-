namespace Pro_FactureAPI.Service.Abonnement
{
    using Pro_FactureAPI.Data;
    using Pro_FactureAPI.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AbonnementService : IAbonnement
    {
        private readonly ProfactureDb _context;

        public AbonnementService(ProfactureDb context)
        {
            _context = context;
        }

        public IEnumerable<Abonnement> GetAll()
        {
            return _context.Abonnements.OrderByDescending(a => a.DateCreation)
                                       .ToList();
        }

        public Abonnement Get(Guid id)
        {
            return _context.Abonnements.Find(id);
        }

        public Abonnement Add(Abonnement abonnement)
        {
            abonnement.DateCreation = DateTime.Now; // Définir la date de création
            _context.Abonnements.Add(abonnement);
            _context.SaveChanges();
            return abonnement;
        }

        public void Remove(Guid id)
        {
            var abonnement = _context.Abonnements.Find(id);
            if (abonnement != null)
            {
                _context.Abonnements.Remove(abonnement);
                _context.SaveChanges();
            }
        }

        public bool Update(Abonnement abonnement)
        {
            var existingAbonnement = _context.Abonnements.Find(abonnement.IdAbonnement);
            if (existingAbonnement == null)
            {
                return false;
            }

            _context.Entry(existingAbonnement).CurrentValues.SetValues(abonnement);
            _context.SaveChanges();
            return true;
        }

        public bool SetInactive(Guid id)
        {
            var abonnement = _context.Abonnements.Find(id);
            if (abonnement == null)
            {
                return false;
            }

            // Basculer l'état actif/inactif
            abonnement.Actif = !abonnement.Actif;
            _context.SaveChanges();
            return true;
        }


    }
}
