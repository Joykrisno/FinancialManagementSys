using MediatR;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Common.Models;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Queries
{
    public class GetChartOfAccountsQuery : IRequest<ApiResponse<PaginatedList<ChartOfAccountDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public bool ActiveOnly { get; set; } = true;
    }
}