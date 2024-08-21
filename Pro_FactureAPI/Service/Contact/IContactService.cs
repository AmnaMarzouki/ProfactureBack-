namespace Pro_FactureAPI.Service.Contact;
using Pro_FactureAPI.Models;


public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task<Contact> AddContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(int id);
        Task<bool> SendReplyEmailAsync(string email, string replyMessage);
    }
