using FinancialManagement.Application.DTOs.JournalEntry;
using MediatR;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class CreateJournalEntryCommandHandler
        : IRequestHandler<CreateJournalEntryCommand, JournalEntryDto>
    {
        public async Task<JournalEntryDto> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var dto = new JournalEntryDto
            {
                Id = 1,
                JournalNumber = request.JournalNumber,
                TransactionDate = request.TransactionDate,
                Description = request.Description,
                Reference = request.Reference,
                UserId = request.UserId,
                IsPosted = request.IsPosted,
                TotalDebit = request.Lines.Sum(x => x.DebitAmount),
                TotalCredit = request.Lines.Sum(x => x.CreditAmount),
                Lines = request.Lines
            };

            return await Task.FromResult(dto);
        }
    }
}
