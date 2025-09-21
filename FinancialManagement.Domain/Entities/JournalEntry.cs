using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Domain.Entities
{
    public class JournalEntry : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string JournalNumber { get; set; } = string.Empty;

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Reference { get; set; } = string.Empty;

        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalCredit { get; set; } = 0;

        public bool IsPosted { get; set; } = false;
        public DateTime? PostedDate { get; set; }
        public string? PostedBy { get; set; }

        public int UserId { get; set; }

        // Navigation properties (temporarily commented)
        public virtual User User { get; set; } = null!;
         public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}