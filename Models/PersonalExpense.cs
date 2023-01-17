using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitPeWebAPI.Models
{
    public class PersonalExpense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public decimal Amount { get; set; }

        
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Category { get; set; }
    }
}
