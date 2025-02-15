namespace Va.Developer.Assessment.Application.Contracts.Services;

public interface ITransactionService
{
    Task<IResponse> Add(TransactionDto transaction);
    Task<IResponse> Delete(TransactionDto transaction);
}
