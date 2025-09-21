using MediatR;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Common.Models;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Commands
{
    public class CreateChartOfAccountCommand : IRequest<ApiResponse<ChartOfAccountDto>>
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
        public decimal OpeningBalance { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
    }
}