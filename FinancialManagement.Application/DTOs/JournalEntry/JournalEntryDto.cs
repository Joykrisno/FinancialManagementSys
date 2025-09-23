using System.Text.Json.Serialization;

namespace FinancialManagement.Application.DTOs.JournalEntry
{
    // -----------------------------
    // Main JournalEntry DTO
    // -----------------------------
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public string JournalNumber { get; set; } = string.Empty;

        [JsonPropertyName("journalDate")]
        public DateTime TransactionDate { get; set; }  // Changed from JournalDate to TransactionDate
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;  // Added Reference field
        public decimal TotalDebit { get; set; }  // Added TotalDebit
        public decimal TotalCredit { get; set; }  // Added TotalCredit
        public int UserId { get; set; }
        public bool IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }  // Added PostedDate
        public string? PostedBy { get; set; }  // Added PostedBy
        public string UserName { get; set; } = string.Empty;  // For display purposes
        public DateTime CreatedDate { get; set; }  // Added CreatedDate
        public DateTime? UpdatedDate { get; set; }  // Added UpdatedDate
        public string CreatedBy { get; set; } = string.Empty;  // Added CreatedBy
        public string? UpdatedBy { get; set; }  // Added UpdatedBy
        public bool IsActive { get; set; } = true;  // Added IsActive
        public bool IsDeleted { get; set; } = false;  // Added IsDeleted

        // Lines of the Journal Entry
        public List<JournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // DTO for creating JournalEntry
    // -----------------------------
    public class CreateJournalEntryDto
    {
        public string JournalNumber { get; set; } = string.Empty;  // Added JournalNumber
        public DateTime TransactionDate { get; set; }  // Changed from JournalDate
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;  // Added CreatedBy
        public List<CreateJournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // DTO for updating JournalEntry
    // -----------------------------
    public class UpdateJournalEntryDto
    {
        public int Id { get; set; }
        public string JournalNumber { get; set; } = string.Empty;  // Added JournalNumber
        public DateTime TransactionDate { get; set; }  // Changed from JournalDate
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; }
        public string? UpdatedBy { get; set; }  // Added UpdatedBy
        public List<UpdateJournalEntryLineDto> Lines { get; set; } = new();
    }

    // -----------------------------
    // Journal Entry Line DTO
    // -----------------------------
    public class JournalEntryLineDto
    {
        public int Id { get; set; }
        public int JournalEntryId { get; set; }  // Added JournalEntryId
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;  // For display purposes
        public string AccountName { get; set; } = string.Empty;  // For display purposes
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int LineNumber { get; set; }
        public DateTime CreatedDate { get; set; }  // Added CreatedDate
        public DateTime? UpdatedDate { get; set; }  // Added UpdatedDate
        public string CreatedBy { get; set; } = string.Empty;  // Added CreatedBy
        public string? UpdatedBy { get; set; }  // Added UpdatedBy
        public bool IsActive { get; set; } = true;  // Added IsActive
        public bool IsDeleted { get; set; } = false;  // Added IsDeleted
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
        public string CreatedBy { get; set; } = string.Empty;  // Added CreatedBy
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
        public string? UpdatedBy { get; set; }  // Added UpdatedBy
    }

    // -----------------------------
    // DTO for posting Journal Entry
    // -----------------------------
    public class PostJournalEntryDto
    {
        public int Id { get; set; }
        public string PostedBy { get; set; } = string.Empty;
    }

    // -----------------------------
    // DTO for Journal Entry summary/list view
    // -----------------------------
    public class JournalEntrySummaryDto
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
        public string CreatedBy { get; set; } = string.Empty;
    }
}