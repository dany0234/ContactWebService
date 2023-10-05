using Microsoft.AspNetCore.Mvc;
using PhoneBookWebService.Models;
using PhoneBookWebService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBookWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactRepository _contactRepository;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(ContactRepository contactRepository, ILogger<ContactsController> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> GetContacts()
        {
            _logger.LogInformation("Fetching all contacts");
            return await _contactRepository.GetAllContactsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContactById(string id) // Changed id to string
        {
            var contact = await _contactRepository.GetContactByIdAsync(id); // Changed id to string
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest("Contact cannot be null");
            }
            await _contactRepository.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact); // Return CreatedAtAction for better RESTfulness
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(string id, [FromBody] Contact contact) // Changed id to string and included it in the method signature
        {
            if (contact == null)
            {
                return BadRequest("Contact cannot be null");
            }
            if (id != contact.Id) // Check if id in URL matches id in the request body
            {
                return BadRequest("Mismatched contact Id");
            }
            await _contactRepository.UpdateContactAsync(contact);
            return NoContent(); // Return NoContent for successful PUT requests as per RESTful principles
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id) // Changed id to string
        {
            var contact = await _contactRepository.GetContactByIdAsync(id); // Verify if contact exists before deletion
            if (contact == null)
            {
                return NotFound(); // Return NotFound if contact does not exist
            }
            await _contactRepository.DeleteContactAsync(id);
            return NoContent(); // Return NoContent for successful DELETE requests as per RESTful principles
        }
    }
}
