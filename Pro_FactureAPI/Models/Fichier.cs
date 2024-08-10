using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace Pro_FactureAPI.Models
    {
        public class Fichier
        {
            [Key]
            public Guid IdFichier { get; set; }
            public string NomFichier { get; set; }
            public string Type { get; set; }
            public DateTime DateImportation { get; set; }

            [ForeignKey("Repertoire")]
            public Guid RepertoireFk { get; set; }
            public virtual Repertoire Repertoire { get; set; }
        }
    }

