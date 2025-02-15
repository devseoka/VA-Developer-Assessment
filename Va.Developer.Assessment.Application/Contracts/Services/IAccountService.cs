namespace Va.Developer.Assessment.Application.Contracts.Services;

public interface IAccountService
{
    IQueryable<AccountDto> Accounts { get; }
    Task<IResponse> Add(AccountDto account);
    Task<IResponse> Update(AccountDto account);
    Task<IResponse> Delete(AccountDto account);
    Task<IEnumerable<AccountDto>> GetAccounts();
    Task<IEnumerable<AccountDto>> GetAccounts(int code);
    Task<AccountDto> GetAccountById(int code);
}
