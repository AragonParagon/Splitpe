using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitPeWebAPI.DB;
using SplitPeWebAPI.Models;

namespace SplitPeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly SplitPeContext _context;

        public ContactsController(SplitPeContext context)
        {
            _context = context;
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            //Contact stiored against user. User must Exist
            if(!UserExists(contact.UserId))
            {
                return BadRequest();
            }

            try
            {
                var localContact = await GetContact(contact.UserId, contact.Number);

                if (localContact is null)
                {
                    //If does not exist, Add
                    _context.Contacts.Add(contact);
                }
                else
                {
                    //If exits, update
                    localContact.Name = contact.Name;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return NoContent();
        }

        private async Task<Contact> GetContact(string userId, string number)
        {
            return await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == userId && c.Number == number);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
