using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitPeWebAPI.Models
{
    public class SplitExpense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [ForeignKey("User")]
        public string OwedTo { get; set; }

        public User User { get; set; }

        [Required]
        [StringLength(10)]
        public string OwedBy { get; set; }

        [Required]
        [StringLength(30)]
        public string OwedByName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; }


        public bool Settled { get; set; } = false;
    }
}
