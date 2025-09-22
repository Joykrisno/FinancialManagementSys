using FinancialManagement.Domain.Entities;
using System.Threading.Tasks;

namespace FinancialManagement.Application.Interfaces
{
    public interface IJournalEntryRepository
    {
        Task<List<JournalEntry>> GetAllWithLinesAsync();
        Task<JournalEntry> AddAsync(JournalEntry entity);
        Task SaveChangesAsync();
        Task<JournalEntry> GetByIdAsync(int id);
      Task<bool> DeleteAsync(int id);
       Task UpdateAsync(JournalEntry entity);
    }
}
