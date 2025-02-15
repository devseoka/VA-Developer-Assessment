using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Domain.Contracts.Repository;

public interface IAccountRepository
{
    IQueryable<Account> Accounts { get; }
    Task<Account> Add(Account account);
    Task<Account> Update(Account account);
    Task<Account> Delete(Account account);
}