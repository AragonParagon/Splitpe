using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitPeWebAPI.Models
{
    public class Due
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
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }


        public bool Settled { get; set; } = false;
    }
}

