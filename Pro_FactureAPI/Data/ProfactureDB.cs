using Microsoft.EntityFrameworkCore;
using Pro_FactureAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;

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
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fichier>()
                .HasOne(f => f.Repertoire)
                .WithMany(r => r.Fichiers)
                .HasForeignKey(f => f.RepertoireFk)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}