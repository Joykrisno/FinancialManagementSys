using MediatR;
using FinancialManagement.Application.DTOs.JournalEntry;
using System.Collections.Generic;

namespace FinancialManagement.Application.Features.JournalEntries.Queries
{
    public class GetJournalEntryListQuery : IRequest<List<JournalEntryDto>>
    {
    }
}
