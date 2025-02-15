using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Domain.Contracts.Repository;

public interface ITransactionRepository
{
    IQueryable<Transaction> Transactions { get; }
    Task<Transaction> Add(Transaction transaction);
    Task<Transaction> Update(Transaction transaction);
    Task<Transaction> Delete(Transaction transaction);
}