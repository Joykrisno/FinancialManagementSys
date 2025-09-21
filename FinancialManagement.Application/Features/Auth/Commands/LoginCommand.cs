using MediatR;
using FinancialManagement.Application.DTOs.Auth;
using FinancialManagement.Application.Common.Models;

namespace FinancialManagement.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<ApiResponse<LoginResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}