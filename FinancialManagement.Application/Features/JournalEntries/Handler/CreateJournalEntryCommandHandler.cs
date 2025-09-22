using FinancialManagement.Application.DTOs.JournalEntry;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using MediatR;

namespace FinancialManagement.Application.Features.JournalEntries.Commands
{
    public class CreateJournalEntryCommandHandler
        : IRequestHandler<CreateJournalEntryCommand, JournalEntryDto>
    {
        private readonly IJournalEntryRepository _repository;

        public CreateJournalEntryCommandHandler(IJournalEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<JournalEntryDto> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = new JournalEntry
            {
                JournalNumber = request.JournalNumber,
                TransactionDate = request.TransactionDate,
                Description = request.Description,
                Reference = request.Reference,
                UserId = request.UserId,
                IsPosted = request.IsPosted,
                TotalDebit = request.Lines.Sum(l => l.DebitAmount),
                TotalCredit = request.Lines.Sum(l => l.CreditAmount),
                Lines = request.Lines.Select(l => new JournalEntryLine
                {
                    AccountId = l.AccountId,
                    Description = l.Description,
                    Reference = l.Reference,
                    DebitAmount = l.DebitAmount,
                    CreditAmount = l.CreditAmount,
                    LineNumber = l.LineNumber,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                }).ToList(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _repository.AddAsync(entry);
            await _repository.SaveChangesAsync();

            // Return DTO
            return new JournalEntryDto
            {
                Id = entry.Id,
                JournalNumber = entry.JournalNumber,
                TransactionDate = entry.TransactionDate,
                Description = entry.Description,
                Reference = entry.Reference,
                UserId = entry.UserId,
                IsPosted = entry.IsPosted,
                TotalDebit = entry.TotalDebit,
                TotalCredit = entry.TotalCredit,
                Lines = entry.Lines.Select(l => new JournalEntryLineDto
                {
                    Id = l.Id,
                    JournalEntryId = l.JournalEntryId,
                    AccountId = l.AccountId,
                    Description = l.Description,
                    Reference = l.Reference,
                    DebitAmount = l.DebitAmount,
                    CreditAmount = l.CreditAmount,
                    LineNumber = l.LineNumber
                }).ToList()
            };
        }
    }
}
