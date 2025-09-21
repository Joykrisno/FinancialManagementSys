namespace FinancialManagement.Domain.Enums
{
    public enum AccountTypes
    {
        Asset = 1,
        Liability = 2,
        Equity = 3,
        Revenue = 4,
        Expense = 5
    }

    public enum UserRoles
    {
        Admin = 1,
        User = 2,
        Viewer = 3
    }

    public enum TransactionTypes
    {
        Debit = 1,
        Credit = 2
    }
}