using FinancialManagement.Application.DTOs.JournalEntry;
using MediatR;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class CreateJournalEntryCommand : IRequest<JournalEntryDto>
    {
        public string JournalNumber { get; set; } = string.Empty;
        public DateTime JournalDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsPosted { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; } = new();
    }
}
