using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Web.Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
