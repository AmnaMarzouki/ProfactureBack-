namespace Pro_FactureAPI.Service.Contact;
using Pro_FactureAPI.Models;


public interface IContactService
    {
    IEnumerable<Contact> GetAllContactsAsync();
    Contact GetContactByIdAsync(Guid id);
    Contact AddContactAsync(Contact contact);
    bool DeleteContactAsync(Guid id);
    bool SendReplyEmailAsync(string email, string replyMessage);
}
