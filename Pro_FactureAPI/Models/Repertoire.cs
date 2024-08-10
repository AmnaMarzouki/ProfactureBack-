using System.ComponentModel.DataAnnotations;

namespace Pro_FactureAPI.Models
{
    public class Repertoire
    {
        [Key]
        public Guid IdRepertoire { get; set; }
        public string NomRepertoire { get; set; }
        public DateTime DateCreation { get; set; }
        public string UtilisateurId { get; set; }
        public virtual ICollection<Fichier> Fichiers { get; set; }
    }
}
