using AutoMapper;
using MediatR;
using FinancialManagement.Application.Features.ChartOfAccounts.Queries;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Handlers
{
    public class GetChartOfAccountsQueryHandler : IRequestHandler<GetChartOfAccountsQuery, ApiResponse<PaginatedList<ChartOfAccountDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetChartOfAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedList<ChartOfAccountDto>>> Handle(GetChartOfAccountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var (data, totalCount) = await _unitOfWork.ChartOfAccounts.GetPagedAsync(
                    pageNumber: request.PageNumber,
                    pageSize: request.PageSize,
                    filter: x => (!request.ActiveOnly || x.IsActive) &&
                                !x.IsDeleted &&
                                (string.IsNullOrEmpty(request.SearchTerm) ||
                                 x.AccountName.Contains(request.SearchTerm) ||
                                 x.AccountCode.Contains(request.SearchTerm)) &&
                                (string.IsNullOrEmpty(request.AccountType) ||
                                 x.AccountType == request.AccountType),
                    orderBy: q => q.OrderBy(x => x.AccountCode),
                    includeProperties: "ParentAccount"
                );

                var accountDtos = _mapper.Map<IEnumerable<ChartOfAccountDto>>(data);
                var paginatedResult = new PaginatedList<ChartOfAccountDto>(accountDtos, totalCount, request.PageNumber, request.PageSize);

                return new ApiResponse<PaginatedList<ChartOfAccountDto>>(paginatedResult, "Chart of Accounts retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedList<ChartOfAccountDto>>($"Error retrieving accounts: {ex.Message}");
            }
        }
    }
}