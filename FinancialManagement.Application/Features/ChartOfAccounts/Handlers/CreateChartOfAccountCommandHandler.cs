using MediatR;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Application.DTOs.ChartOfAccount;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Queries
{
    public class GetChartOfAccountByIdQuery : IRequest<ApiResponse<ChartOfAccountDto>>
    {
        public int Id { get; set; }
    }
}
