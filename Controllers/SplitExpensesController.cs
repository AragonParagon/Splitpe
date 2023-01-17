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
    public class SplitExpensesController : ControllerBase
    {
        private readonly SplitPeContext _context;

        public SplitExpensesController(SplitPeContext context)
        {
            _context = context;
        }

        // GET: api/SplitExpenses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<SplitExpense>>> GetSplitExpenses()
        //{
        //    return await _context.SplitExpenses.ToListAsync();
        //}

        //// GET: api/SplitExpenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSplitExpense(string id)
        {

            var settlementDetails = new SettlementDetails
            {
                IOweTo = _context.OweTos.FromSqlRaw($"Select f.OwedTo,Name = (select top 1 * from (select Name from Contacts where UserId = {id} and Number = f.OwedTo union select Email as Name from Users where id = f.OwedTo) u), f.NetAmount from ( select a.OwedTo, sum(a.Amount) - (select COALESCE(sum(b.Amount),0) from SplitExpenses b where b.OwedTo = {id} and b.OwedBy = a.OwedTo and Settled=0) as NetAmount from [dbo].[SplitExpenses] a where a.[OwedBy] = {id} and a.Settled=0 group by a.OwedTo) as f where f.NetAmount > 0").ToList(),
                TheyOweMe = _context.OweBys.FromSqlRaw($"Select f.OwedBy,Name = (select top 1 * from (select Name from Contacts where UserId = {id} and Number = f.OwedBy union select Email as Name from Users where id = f.OwedBy) u), f.NetAmount from ( select a.OwedBy, sum(a.Amount) - (select COALESCE(sum(b.Amount),0) from SplitExpenses b where b.OwedBy = {id} and b.OwedTo = a.OwedBy  and Settled=0) as NetAmount from [dbo].[SplitExpenses] a where a.[OwedTo] = {id}  and a.Settled=0 group by a.OwedBy) as f where f.NetAmount > 0").ToList()

            };
            //var splitExpense = await _context.SplitExpenses.FindAsync(id);

            //if (splitExpense == null)
            //{
            //    return NotFound();
            //}

            return Ok(settlementDetails);
        }

        /// PUT: api/SplitExpenses/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSplitExpense(int id, SplitExpense splitExpense)
        ////{
        //    if (id != splitExpense.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(splitExpense).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SplitExpenseExists(id))
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

        // POST: api/SplitExpenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SplitExpense>> PostSplitExpense(SplitExpenses splitExpenses)
        {
            decimal equalAmount = 0;
            decimal userAmount = 0;
            PersonalExpense userShare = null;
            if(splitExpenses.SplitType.ToLower().Equals("equal"))
            {
                equalAmount = splitExpenses.TotalAmount / splitExpenses.NumberOfPeople;
                foreach(SplitExpense person in splitExpenses.IndividualExpenses)
                {
                    person.Amount = equalAmount;
                }
                var user = _context.Users.Find(splitExpenses.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.RemainingBudget = user.RemainingBudget - equalAmount;
                userShare = new PersonalExpense { UserId = splitExpenses.UserId, Amount = equalAmount, Category = splitExpenses.Category };
            }
            else if(splitExpenses.SplitType.ToLower().Equals("unequal"))
            {
                decimal individualTotal = 0;
                foreach (SplitExpense person in splitExpenses.IndividualExpenses)
                {
                   individualTotal += person.Amount;
                }
                userAmount = splitExpenses.TotalAmount - individualTotal;
                var user = _context.Users.Find(splitExpenses.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.RemainingBudget = user.RemainingBudget - userAmount;
                userShare = new PersonalExpense { UserId = splitExpenses.UserId, Amount = userAmount, Category = splitExpenses.Category };

            }

  
            foreach(SplitExpense individualShare in splitExpenses.IndividualExpenses)
            {
                if(!_context.Contacts.FromSqlRaw($"select * from Contacts where UserId = {individualShare.OwedTo} and Number = {individualShare.OwedBy}").Any())
                {
                    Contact userContact = new Contact { UserId = individualShare.OwedTo, Name = individualShare.OwedByName, Number = individualShare.OwedBy };
                    _context.Contacts.Add(userContact);
                }
                _context.SplitExpenses.Add(individualShare);
            }
            
            _context.PersonalExpenses.Add(userShare);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/SplitExpenses/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSplitExpense(int id)
        //{
        //    var splitExpense = await _context.SplitExpenses.FindAsync(id);
        //    if (splitExpense == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SplitExpenses.Remove(splitExpense);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool SplitExpenseExists(int id)
        //{
        //    return _context.SplitExpenses.Any(e => e.Id == id);
        //}
    }
}
