
namespace Pro_FactureAPI.Service.Contact
{
    public class ContactService : IContactService
    {
        public Task<Models.Contact> AddContactAsync(Models.Contact contact)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteContactAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Models.Contact>> GetAllContactsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Models.Contact> GetContactByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendReplyEmailAsync(string email, string replyMessage)
        {
            throw new NotImplementedException();
        }
    }
}
