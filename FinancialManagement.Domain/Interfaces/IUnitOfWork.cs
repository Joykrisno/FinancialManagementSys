using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<ChartOfAccount> ChartOfAccounts { get; }
        IGenericRepository<JournalEntry> JournalEntries { get; }
        IGenericRepository<JournalEntryLine> JournalEntryLines { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}