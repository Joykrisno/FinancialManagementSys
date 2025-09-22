using MediatR;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class DeleteJournalEntryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
