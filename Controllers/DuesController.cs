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
    public class DuesController : ControllerBase
    {
        private readonly SplitPeContext _context;

        public DuesController(SplitPeContext context)
        {
            _context = context;
        }

        // GET: api/Dues
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Due>>> GetDues()
        //{
        //    return await _context.Dues.ToListAsync();
        //}

        //// GET: api/Dues/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Due>> GetDue(int id)
        //{
        //    var due = await _context.Dues.FindAsync(id);

        //    if (due == null)
        //    {
        //        return NotFound();
        //    }

        //    return due;
        //}

        //// PUT: api/Dues/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDue(int id, Due due)
        //{
        //    if (id != due.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(due).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DueExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Dues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Due>> PostDue(SplitExpense due)
        {
            if (!_context.Contacts.FromSqlRaw($"select * from Contacts where UserId = {due.OwedTo} and Number = {due.OwedBy}").Any())
            {
                Contact userContact = new Contact { UserId = due.OwedTo, Name = due.OwedByName, Number = due.OwedBy };
                _context.Contacts.Add(userContact);
            }
            _context.SplitExpenses.Add(due);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Dues/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteDue(int id)
        //    {
        //        var due = await _context.Dues.FindAsync(id);
        //        if (due == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Dues.Remove(due);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //    private bool DueExists(int id)
        //    {
        //        return _context.Dues.Any(e => e.Id == id);
        //    }
        //}
    }
}
