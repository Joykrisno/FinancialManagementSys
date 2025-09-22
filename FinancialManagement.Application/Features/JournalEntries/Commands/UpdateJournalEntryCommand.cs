using MediatR;
using FinancialManagement.Application.DTOs.JournalEntry;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class UpdateJournalEntryCommand : IRequest<JournalEntryDto>
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public List<UpdateJournalEntryLineDto> Lines { get; set; } = new();
    }
}
