namespace Va.Developer.Assessment.Application.Contracts.Services;

public interface ITransactionService
{
    IQueryable<TransactionDto> Transactions { get; }
    Task<IResponse> Add(TransactionDto transaction);
    Task<IResponse> Update(TransactionDto transaction);
    Task<IResponse> Delete(TransactionDto transaction);
    Task<TransactionDto> GetTransactionById(int code);
}
