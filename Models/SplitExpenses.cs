using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitPeWebAPI.Models
{
    public class SplitExpenses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string SplitType { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; }

        [Required]
        public List<SplitExpense> IndividualExpenses { get; set; }

    }
}
