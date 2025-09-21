using Microsoft.EntityFrameworkCore.Storage;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Infrastructure.Data;

namespace FinancialManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Repository instances
        private IGenericRepository<User>? _users;
        private IGenericRepository<ChartOfAccount>? _chartOfAccounts;
        private IGenericRepository<JournalEntry>? _journalEntries;
        private IGenericRepository<JournalEntryLine>? _journalEntryLines;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<User> Users
        {
            get { return _users ??= new GenericRepository<User>(_context); }
        }

        public IGenericRepository<ChartOfAccount> ChartOfAccounts
        {
            get { return _chartOfAccounts ??= new GenericRepository<ChartOfAccount>(_context); }
        }

        public IGenericRepository<JournalEntry> JournalEntries
        {
            get { return _journalEntries ??= new GenericRepository<JournalEntry>(_context); }
        }

        public IGenericRepository<JournalEntryLine> JournalEntryLines
        {
            get { return _journalEntryLines ??= new GenericRepository<JournalEntryLine>(_context); }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}