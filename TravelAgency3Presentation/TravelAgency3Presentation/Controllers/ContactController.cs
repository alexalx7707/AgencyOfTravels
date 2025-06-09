using Microsoft.AspNetCore.Mvc;
using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Services;

namespace TravelAgency3Presentation.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                await _contactService.AddContactAsync(contact);
                TempData["SuccessMessage"] = "Your message has been sent successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", contact);
        }

        // GET: Contact/Admin
        public async Task<IActionResult> Admin()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return View(contacts);
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/MarkAsResolved/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            await _contactService.MarkAsResolvedAsync(id);
            return RedirectToAction(nameof(Admin));
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _contactService.DeleteContactAsync(id);
            return RedirectToAction(nameof(Admin));
        }
    }
}
