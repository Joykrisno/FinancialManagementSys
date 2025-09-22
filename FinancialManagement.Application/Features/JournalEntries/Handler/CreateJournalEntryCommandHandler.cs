using FinancialManagement.Application.DTOs.JournalEntry;
using MediatR;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class CreateJournalEntryCommandHandler
        : IRequestHandler<CreateJournalEntryCommand, JournalEntryDto>
    {
        public Task<JournalEntryDto> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            // 👉 এখানে তোমার Infrastructure layer (Repository/DbContext) use করবে
            // Example static response (DB integration পরে করবে)
            var dto = new JournalEntryDto
            {
                Id = 1,
                JournalNumber = request.JournalNumber,
                JournalDate = request.JournalDate,
                Description = request.Description,
                UserId = request.UserId,
                IsPosted = request.IsPosted,
                Lines = request.Lines
            };

            return Task.FromResult(dto);
        }
    }
}
