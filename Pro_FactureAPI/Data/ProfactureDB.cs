using Microsoft.EntityFrameworkCore;
using Pro_FactureAPI.Models;
using System.Collections.Generic;

namespace Pro_FactureAPI.Data
{

    public class ProfactureDb : DbContext
        {
            public ProfactureDb(DbContextOptions<ProfactureDb> options) : base(options)
            {
            }
            public DbSet<Repertoire> Repertoires { get; set; }
            public DbSet<Fichier> Fichiers { get; set; }
           public DbSet<Abonnement> Abonnements { get; set; }
    }
    
}
