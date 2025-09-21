using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User> RegisterUserAsync(User user, string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}