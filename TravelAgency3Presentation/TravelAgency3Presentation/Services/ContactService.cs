using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Repositories;

namespace TravelAgency3Presentation.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact> _contactRepository;

        public ContactService(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            contact.CreatedAt = DateTime.UtcNow;
            contact.IsResolved = false;
            return await _contactRepository.AddAsync(contact);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            await _contactRepository.UpdateAsync(contact);
        }

        public async Task DeleteContactAsync(int id)
        {
            await _contactRepository.DeleteAsync(id);
        }

        public async Task<bool> MarkAsResolvedAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null)
                return false;

            contact.IsResolved = true;
            await _contactRepository.UpdateAsync(contact);
            return true;
        }
    }
}
