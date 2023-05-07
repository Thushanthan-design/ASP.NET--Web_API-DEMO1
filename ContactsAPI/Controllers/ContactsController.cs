using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
	[ApiController]
	[Route("api/Contacts")]
	public class ContactsController : Controller
	{
		private readonly ContactsAPIDbContext dbContext;

		public ContactsController(ContactsAPIDbContext dbContext)
        {
			this.dbContext = dbContext;
		}

        [HttpGet]
		public async Task<IActionResult> GetContact()
		{
			return Ok(await dbContext.Contacts.ToListAsync());
		}

		[HttpGet]
		[Route("{id}")]

		public async Task<IActionResult> GetContact([FromRoute] Guid id)
		{
			var contact = await dbContext.Contacts.FindAsync(id);

			if(contact == null) { 
				return NotFound();
			
			}
			return Ok(contact);
		}

		[HttpPost]
		public async Task<IActionResult> AddContact(AddContactRequest addContactRequest )
		{
			var contact = new Contact()
			{
				Id = Guid.NewGuid(),
				FullName = addContactRequest.FullName,
				Email = addContactRequest.Email,
				Phone = addContactRequest.Phone,
				Address = addContactRequest.Address
			};

			await dbContext.Contacts.AddAsync(contact);
			await dbContext.SaveChangesAsync();
			return Ok(contact);

		}

		[HttpPut]
		[Route("{id}")]

		public async Task<IActionResult> UpdateContact([FromRoute] Guid id,  UpdateContactRequest updateContactRequest)
		{
			var  contact = await dbContext.Contacts.FindAsync(id);
			if(contact != null)
			{
				contact.Email = updateContactRequest.Email;
				contact.Phone = updateContactRequest.Phone;
				contact.Address = updateContactRequest.Address;
				contact.FullName = updateContactRequest.FullName;

				dbContext.SaveChangesAsync();
				return Ok(contact);

			}
			return NotFound();
		}

		[HttpDelete]
		[Route("{id}")]

		public async  Task<IActionResult> DeleteContact([FromRoute] Guid id)
		{
			var contact = await dbContext.Contacts.FindAsync(id);
			if(contact != null)
			{
				dbContext.Remove(contact);
				await dbContext.SaveChangesAsync();
				return Ok(contact);
			}

			return NotFound();
		}

	}
}
