using Microsoft.AspNetCore.Mvc;
using Pro_FactureAPI.Models;
using Pro_FactureAPI.Service.Contact;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pro_FactureAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: api/Contact
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetAllContacts()
        {
            var contacts = _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        // GET: api/Contact/{id}
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContactById(Guid id)
        {
            var contact = _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // POST: api/Contact
        [HttpPost]
        public ActionResult<Contact> CreateContact(Contact contact)
        {
            var createdContact = _contactService.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContactById), new { id = createdContact.IdContact }, createdContact);
        }

        // DELETE: api/Contact/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(Guid id)
        {
            var success = _contactService.DeleteContactAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Contact/reply
        [HttpPost("reply")]
        public IActionResult SendReplyEmail([FromBody] ReplyEmailDto dto)
        {
            var success = _contactService.SendReplyEmailAsync(dto.Email, dto.ReplyMessage);
            if (!success)
            {
                return StatusCode(500, "An error occurred while sending the email.");
            }

            return Ok("Reply email sent successfully.");
        }
    }

    // DTO for sending reply emails
    public class ReplyEmailDto
    {
        public string Email { get; set; }
        public string ReplyMessage { get; set; }
    }
}