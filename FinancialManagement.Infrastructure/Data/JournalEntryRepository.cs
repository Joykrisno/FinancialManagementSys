using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinancialManagement.Infrastructure.Data
{
    public class JournalEntryRepository : IJournalEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public JournalEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JournalEntry> AddAsync(JournalEntry entity)
        {
            await _context.JournalEntries.AddAsync(entity);
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<JournalEntry> GetByIdAsync(int id)
        {
            return await _context.JournalEntries
                                 .Include(j => j.Lines)
                                 .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.JournalEntries.FindAsync(id);
            if (entity == null) return false;

            _context.JournalEntries.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(JournalEntry entity)
        {
            _context.JournalEntries.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
