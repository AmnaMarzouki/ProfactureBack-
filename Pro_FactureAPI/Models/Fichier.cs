using System.ComponentModel.DataAnnotations;

namespace Pro_FactureAPI.Models
{
    public class Fichier
    {

        [Key]
        public Guid IdFichier { get; set; }
        public string NomFichier { get; set; }

        public string Type { get; set; }
        public DateTime DateImportation { get; set; }

    }
}
