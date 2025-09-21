using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Domain.Entities
{
    public class JournalEntryLine : BaseEntity
    {
        public int JournalEntryId { get; set; }
        public int AccountId { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Reference { get; set; } = string.Empty;

        public decimal DebitAmount { get; set; } = 0;
        public decimal CreditAmount { get; set; } = 0;

        public int LineNumber { get; set; } = 1;

        // Navigation properties
        public virtual JournalEntry JournalEntry { get; set; } = null!;
        public virtual ChartOfAccount Account { get; set; } = null!;
    }
}