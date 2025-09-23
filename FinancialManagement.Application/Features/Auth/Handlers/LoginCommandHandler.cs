using MediatR;
using FinancialManagement.Application.Features.Auth.Commands;
using FinancialManagement.Application.DTOs.Auth;
using FinancialManagement.Application.Common.Models;
using FinancialManagement.Domain.Interfaces;

namespace FinancialManagement.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ApiResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authService.ValidateUserAsync(request.Email, request.Password);

                if (user == null)
                {
                    return new ApiResponse<LoginResponseDto>("Invalid email or password");
                }

                var token = _authService.GenerateJwtToken(user);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = user.Role,
                  //  ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                return new ApiResponse<LoginResponseDto>(response, "Login successful");
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponseDto>($"Login failed: {ex.Message}");
            }
        }
    }
}