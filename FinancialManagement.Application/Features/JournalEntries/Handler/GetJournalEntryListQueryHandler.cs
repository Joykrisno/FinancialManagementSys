using FinancialManagement.Application.DTOs.JournalEntry;
using FinancialManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialManagement.Application.Features.JournalEntries.Queries
{
    public class GetJournalEntryListQueryHandler
        : IRequestHandler<GetJournalEntryListQuery, List<JournalEntryDto>>
    {
        private readonly IJournalEntryRepository _repository;

        public GetJournalEntryListQueryHandler(IJournalEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JournalEntryDto>> Handle(GetJournalEntryListQuery request, CancellationToken cancellationToken)
        {
            var entries = await _repository.GetAllWithLinesAsync();

            return entries.Select(e => new JournalEntryDto
            {
                Id = e.Id,
                JournalNumber = e.JournalNumber,
                TransactionDate = e.TransactionDate,
                Description = e.Description,
                Reference = e.Reference,
                UserId = e.UserId,
                TotalDebit = e.TotalDebit,
                TotalCredit = e.TotalCredit,
                Lines = e.Lines.Select(l => new JournalEntryLineDto
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
            }).ToList();
        }
    }
}
