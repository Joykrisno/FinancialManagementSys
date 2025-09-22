using MediatR;
using FinancialManagement.Application.DTOs.JournalEntry;

namespace FinancialManagement.Application.Features.JournalEntries.Queries
{
    public class GetJournalEntryByIdQuery : IRequest<JournalEntryDto>
    {
        public int Id { get; set; }
    }
}
