using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitPeWebAPI.DB;
using SplitPeWebAPI.Models;

namespace SplitPeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettledController : ControllerBase
    {
        private readonly SplitPeContext _context;

        public SettledController(SplitPeContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<IActionResult> PutSettled(Settled settled)
        {

            var rows = await _context.Database.ExecuteSqlAsync($"UPDATE SplitExpenses set Settled = 1 where ((OwedTo = {settled.OwedTo} and OwedBy = {settled.OwedBy}) or (OwedTo = {settled.OwedBy} and OwedBy = {settled.OwedTo})) and Settled=0");
            if(rows == 0)
            {
                return BadRequest();
            }

            return NoContent();
        }


    }
}
