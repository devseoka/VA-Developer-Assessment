using Va.Developer.Assessment.Domain.Contracts.Repository;
using Va.Developer.Assessment.Domain.Models;
using Va.Developer.Assessment.Infrastructure.Persistence.Context;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Repositories;

public class TransactionRepository(VaDeveloperContext context) : ITransactionRepository
{
    private readonly VaDeveloperContext _context = context;
    public IQueryable<Transaction> Transactions => 
        _context
        .Transactions
        .AsTracking();

    public async Task<Transaction> Add(Transaction transaction)
    {
        var entry = _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<Transaction> Delete(Transaction transaction)
    {
        var entry = _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<Transaction> Update(Transaction transaction)
    {
        var entry = _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }
}