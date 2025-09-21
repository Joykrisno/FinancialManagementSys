using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Application.DTOs.JournalEntry
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string JournalNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public bool IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }
        public string? PostedBy { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public List<JournalEntryLineDto> JournalEntryLines { get; set; } = new List<JournalEntryLineDto>();
    }

    public class CreateJournalEntryDto
    {
        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Reference cannot exceed 100 characters")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one journal entry line is required")]
        [MinLength(2, ErrorMessage = "At least two journal entry lines are required")]
        public List<CreateJournalEntryLineDto> JournalEntryLines { get; set; } = new List<CreateJournalEntryLineDto>();
    }

    public class UpdateJournalEntryDto
    {
        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Reference cannot exceed 100 characters")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one journal entry line is required")]
        [MinLength(2, ErrorMessage = "At least two journal entry lines are required")]
        public List<UpdateJournalEntryLineDto> JournalEntryLines { get; set; } = new List<UpdateJournalEntryLineDto>();
    }

    public class JournalEntryLineDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int LineNumber { get; set; }
    }

    public class CreateJournalEntryLineDto
    {
        [Required(ErrorMessage = "Account is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid account")]
        public int AccountId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Reference cannot exceed 100 characters")]
        public string Reference { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Debit Amount must be non-negative")]
        public decimal DebitAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Credit Amount must be non-negative")]
        public decimal CreditAmount { get; set; }

        public int LineNumber { get; set; } = 1;
    }

    public class UpdateJournalEntryLineDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid account")]
        public int AccountId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Reference cannot exceed 100 characters")]
        public string Reference { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Debit Amount must be non-negative")]
        public decimal DebitAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Credit Amount must be non-negative")]
        public decimal CreditAmount { get; set; }

        public int LineNumber { get; set; } = 1;
    }
}