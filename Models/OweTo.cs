using Microsoft.EntityFrameworkCore;

namespace SplitPeWebAPI.Models
{
    [Keyless]
    public class OweTo
    {
        public string OwedTo { get; set; }

        public string Name { get; set; }
        public decimal NetAmount { get; set; }
    }
}
