namespace FinancialManagement.Application.DTOs.JournalEntry
{
    // -----------------------------
    // Main JournalEntry DTO
    // -----------------------------
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string JournalNumber { get; set; } = string.Empty;
        public DateTime JournalDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; }
        public string UserName { get; set; } = string.Empty;

        // Lines of the Journal Entry
        public List<JournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // DTO for creating JournalEntry lines
    // -----------------------------
    public class CreateJournalEntryLineDto
    {
        public int AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int LineNumber { get; set; }
    }

    // -----------------------------
    // DTO for updating JournalEntry lines
    // -----------------------------
    public class UpdateJournalEntryLineDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int LineNumber { get; set; }
    }

    // -----------------------------
    // DTO for creating JournalEntry
    // -----------------------------
    public class CreateJournalEntryDto
    {
        public DateTime JournalDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; }

        public List<CreateJournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // DTO for updating JournalEntry
    // -----------------------------
    public class UpdateJournalEntryDto
    {
        public int Id { get; set; }
        public DateTime JournalDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; }

        public List<UpdateJournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // DTO for JournalEntry lines when returning data
    // -----------------------------
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
}
