using Va.Developer.Assessment.Domain.Contracts.Repository;
using Va.Developer.Assessment.Domain.Models;
using Va.Developer.Assessment.Infrastructure.Persistence.Context;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Repositories
{
    public class AccountRepository(VaDeveloperContext context) : IAccountRepository
    {
        private readonly VaDeveloperContext _context = context;
        public IQueryable<Account> Accounts => 
            _context
            .Accounts
            .AsTracking();

        public async Task<Account> Add(Account account)
        {
            var entry = _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Account> Delete(Account account)
        {
            var entry = _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Account> Update(Account account)
        {
            var entry = _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}