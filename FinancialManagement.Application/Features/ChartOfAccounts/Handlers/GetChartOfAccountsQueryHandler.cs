using AutoMapper;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Features.ChartOfAccounts.Queries;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using MediatR;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Handlers
{
    public class GetChartOfAccountByIdQueryHandler : IRequestHandler<GetChartOfAccountByIdQuery, ApiResponse<ChartOfAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetChartOfAccountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ChartOfAccountDto>> Handle(GetChartOfAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.ChartOfAccounts.GetByIdAsync(request.Id);
            if (account == null)
                return new ApiResponse<ChartOfAccountDto>($"ChartOfAccount with Id={request.Id} not found");

            var dto = _mapper.Map<ChartOfAccountDto>(account);
            return new ApiResponse<ChartOfAccountDto>(dto, "Chart of Account retrieved successfully");
        }
    }
}
