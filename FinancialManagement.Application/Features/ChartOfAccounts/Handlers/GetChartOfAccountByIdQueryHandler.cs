using AutoMapper;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Domain.Interfaces;
using MediatR;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Queries
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
            var entity = await _unitOfWork.ChartOfAccounts.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return new ApiResponse<ChartOfAccountDto>((ChartOfAccountDto?)null)
                {
                    Success = false,
                    Message = "Chart of Account not found"
                };
            }

            var dto = _mapper.Map<ChartOfAccountDto>(entity);

            return new ApiResponse<ChartOfAccountDto>(dto)
            {
                Success = true,
                Message = "Chart of Account retrieved successfully"
            };
        }
    }
}
