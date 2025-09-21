using AutoMapper;
using MediatR;
using FinancialManagement.Application.Features.ChartOfAccounts.Commands;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Application.Features.ChartOfAccounts.Handlers
{
    public class CreateChartOfAccountCommandHandler : IRequestHandler<CreateChartOfAccountCommand, ApiResponse<ChartOfAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateChartOfAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ChartOfAccountDto>> Handle(CreateChartOfAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if account code already exists
                var existingAccount = await _unitOfWork.ChartOfAccounts.GetSingleAsync(x => x.AccountCode == request.AccountCode && !x.IsDeleted);
                if (existingAccount != null)
                {
                    return new ApiResponse<ChartOfAccountDto>("Account code already exists");
                }

                // Validate parent account if provided
                if (request.ParentAccountId.HasValue)
                {
                    var parentAccount = await _unitOfWork.ChartOfAccounts.GetByIdAsync(request.ParentAccountId.Value);
                    if (parentAccount == null || !parentAccount.IsActive || parentAccount.IsDeleted)
                    {
                        return new ApiResponse<ChartOfAccountDto>("Invalid parent account");
                    }
                }

                var account = new ChartOfAccount
                {
                    AccountCode = request.AccountCode,
                    AccountName = request.AccountName,
                    AccountType = request.AccountType,
                    Description = request.Description,
                    ParentAccountId = request.ParentAccountId,
                    OpeningBalance = request.OpeningBalance,
                    CurrentBalance = request.OpeningBalance,
                    Level = request.ParentAccountId.HasValue ? 2 : 1, // Simple level calculation
                    IsParent = false,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                await _unitOfWork.ChartOfAccounts.AddAsync(account);
                await _unitOfWork.CompleteAsync();

                var accountDto = _mapper.Map<ChartOfAccountDto>(account);
                return new ApiResponse<ChartOfAccountDto>(accountDto, "Chart of Account created successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ChartOfAccountDto>($"Error creating account: {ex.Message}");
            }
        }
    }
}