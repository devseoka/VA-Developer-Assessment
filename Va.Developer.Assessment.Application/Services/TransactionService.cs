
using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Application.Services
{
    public class TransactionService(IAccountService accountService,ITransactionManager transactionManager, ITransactionRepository transactionRepository, IMapper mapper) : ITransactionService
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly ITransactionManager  _transactionManager = transactionManager;
        private readonly IMapper _mapper = mapper;
        public async Task<IResponse> Add(TransactionDto transaction)
        {
            await _transactionManager.BeginDatabaseTransactionAsync();
            try
            {
                var account = await _accountService.GetAccountById(transaction.AccountId);
                if (account is null)
                {
                    string message = "Selected account for the transaction is closed or deleted";
                    return new ErrorResponse { Errors = [message], Message = message, Succeeded = false };
                }
                if (transaction.Total == 0)
                {
                    string message = "The transaction amount can never be zero.";
                    return new ErrorResponse { Errors = [message], Message = message, Succeeded = false };
                }
                var entity = _mapper.Map<Transaction>(transaction);

                account.Balance += transaction.Total;
                await _accountService.Update(account);
                
                entity = await _transactionRepository.Add(entity);
                string transactionType = transaction.Total < 0 ?
                    "debit" : "credit";

                await _transactionManager.CommitTransactionAsync();

                return new Response<TransactionDto>
                {
                    Succeeded = entity.Code > 0,
                    Data = _mapper.Map<TransactionDto>(entity),
                    Message = $"You have successfully added a {transactionType} transaction."
                };
            }
            catch(Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                throw;
            }
        }

        public Task<IResponse> Delete(TransactionDto transaction)
        {
            throw new NotImplementedException();
        }
    }
}
