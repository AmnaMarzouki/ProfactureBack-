using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pro_FactureAPI.Models
{
    public class Abonnement
    {
      

        public enum AbonnementType
        {
            Semestriel,
            Mensuel,
            Annuel
        }

        [Key]
        public Guid IdAbonnement { get; set; }
        public DateTime DateCreation { get; set; }

        public bool Publish { get; set; }


        public DateTime DateFin { get; set; }


        public AbonnementType Titre { get; set; }


        public float Coût { get; set; }


        public string Durée { get; set; }


        public bool Actif { get; set; }

        public int NbFichiers { get; set; }

    }
}
