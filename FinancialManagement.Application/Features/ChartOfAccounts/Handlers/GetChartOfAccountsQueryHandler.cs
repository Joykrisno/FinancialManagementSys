using AutoMapper;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using MediatR;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Queries
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

        public async Task<ApiResponse<PaginatedList<ChartOfAccountDto>>> Handle(
            GetChartOfAccountsQuery request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Fetch all ChartOfAccounts from repository
            var allItems = await _unitOfWork.ChartOfAccounts.GetAllAsync();

            // 2️⃣ Apply pagination
            var pagedItems = allItems
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // 3️⃣ Map entities to DTOs
            var dtoList = _mapper.Map<List<ChartOfAccountDto>>(pagedItems);

            // 4️⃣ Prepare paginated list (Positional arguments)
            var paginatedList = new PaginatedList<ChartOfAccountDto>(
        dtoList,
        allItems.Count(),  // method call with ()
        request.PageNumber,
        request.PageSize
    );


            // 5️⃣ Return wrapped ApiResponse
            return new ApiResponse<PaginatedList<ChartOfAccountDto>>(paginatedList)
            {
                Success = true,
                Message = "Chart of Accounts retrieved successfully"
            };
        }
    }
}
