using System.ComponentModel.DataAnnotations;

namespace Pro_FactureAPI.Models
{
    public class Contact
    {
        
        public int Id { get; set; }

    
        public string Nom { get; set; }

        public string Email { get; set; }

       
        public string Sujet { get; set; }


        public string Message { get; set; }
    }
}