using TravelAgency3Presentation.Models;

namespace TravelAgency3Presentation.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task<Contact> AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
        Task<bool> MarkAsResolvedAsync(int id);
    }
}
