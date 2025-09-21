using System.ComponentModel.DataAnnotations;

namespace FinancialManagement.Application.DTOs.ChartOfAccount
{
    public class ChartOfAccountDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account Code is required")]
        [StringLength(20, ErrorMessage = "Account Code cannot exceed 20 characters")]
        public string AccountCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(200, ErrorMessage = "Account Name cannot exceed 200 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }
        public string? ParentAccountName { get; set; }

        public bool IsParent { get; set; } = false;
        public int Level { get; set; } = 1;

        [Range(0, double.MaxValue, ErrorMessage = "Opening Balance must be non-negative")]
        public decimal OpeningBalance { get; set; } = 0;

        public decimal CurrentBalance { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateChartOfAccountDto
    {
        [Required(ErrorMessage = "Account Code is required")]
        [StringLength(20, ErrorMessage = "Account Code cannot exceed 20 characters")]
        public string AccountCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(200, ErrorMessage = "Account Name cannot exceed 200 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Opening Balance must be non-negative")]
        public decimal OpeningBalance { get; set; } = 0;
    }

    public class UpdateChartOfAccountDto
    {
        [Required(ErrorMessage = "Account Code is required")]
        [StringLength(20, ErrorMessage = "Account Code cannot exceed 20 characters")]
        public string AccountCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(200, ErrorMessage = "Account Name cannot exceed 200 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Opening Balance must be non-negative")]
        public decimal OpeningBalance { get; set; } = 0;
    }
}