using Microsoft.EntityFrameworkCore;

namespace SplitPeWebAPI.Models
{
    [Keyless]
    public class OweBy
    {
        public string OwedBy { get; set; }

        public string Name { get; set; }
        public decimal NetAmount { get; set; }
    }
}
