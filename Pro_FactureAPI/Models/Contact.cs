

using System.ComponentModel.DataAnnotations;

namespace Pro_FactureAPI.Models
{
    public class Contact
    {
        [Key]

        public Guid IdContact { get; set; }

    
        public string Nom { get; set; }

        public string Email { get; set; }

       
        public string Sujet { get; set; }


        public string Message { get; set; }
    }
}