using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Domain.Entities
{
    public class ChartOfAccount : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string AccountCode { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AccountType { get; set; } = string.Empty; // Asset, Liability, Equity, Revenue, Expense

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }

        public bool IsParent { get; set; } = false;

        public int Level { get; set; } = 1;

        public decimal OpeningBalance { get; set; } = 0;

        public decimal CurrentBalance { get; set; } = 0;


        public virtual ChartOfAccount? ParentAccount { get; set; }
        public virtual ICollection<ChartOfAccount> ChildAccounts { get; set; } = new List<ChartOfAccount>();
         public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    }
}