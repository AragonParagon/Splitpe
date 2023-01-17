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
    public class PersonalExpensesController : ControllerBase
    {
        private readonly SplitPeContext _context;

        public PersonalExpensesController(SplitPeContext context)
        {
            _context = context;
        }


        // POST: api/PersonalExpenses
        
        [HttpPost]
        public async Task<ActionResult<PersonalExpense>> PostPersonalExpense(PersonalExpense personalExpense)
        {
            var user = _context.Users.Find(personalExpense.UserId);
            if(user == null)
            {
                return NotFound();
            }
            user.RemainingBudget = user.RemainingBudget - personalExpense.Amount;
            _context.PersonalExpenses.Add(personalExpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/PersonalExpenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalExpense(int id)
        {
            var personalExpense = await _context.PersonalExpenses.FindAsync(id);
            if (personalExpense == null)
            {
                return NotFound();
            }
            var user = _context.Users.Find(personalExpense.UserId);
            if (user == null)
            {
                return NotFound();
            }
            user.RemainingBudget = user.RemainingBudget + personalExpense.Amount;
            _context.PersonalExpenses.Remove(personalExpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonalExpenseExists(int id)
        {
            return _context.PersonalExpenses.Any(e => e.Id == id);
        }
    }
}
