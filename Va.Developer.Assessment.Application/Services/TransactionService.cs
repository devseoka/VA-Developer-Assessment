
using Microsoft.Extensions.Logging;
using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Application.Services
{
    public class TransactionService(IAccountService accountService, ITransactionManager transactionManager, ITransactionRepository transactionRepository, IMapper mapper, ILogger<TransactionService> logger) : ITransactionService
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly ITransactionManager  _transactionManager = transactionManager;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TransactionService> _logger = logger;

        public IQueryable<TransactionDto> Transactions =>
            _transactionRepository
            .Transactions
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider);

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
                _logger.LogError(ex, "An unexpected error occured while adding a transaction to {AccountCode}. Error: {Message}", transaction.AccountId, ex.Message);
                await _transactionManager.RollbackTransactionAsync();
                throw;
            }
        }

        public Task<IResponse> Delete(TransactionDto transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionDto> GetTransactionById(int code)
        {
            return await Transactions.FirstOrDefaultAsync(t => t.Id == code);
        }

        public async Task<IResponse> Update(TransactionDto transaction)
        {
            await _transactionManager.BeginDatabaseTransactionAsync();
            try
            {

                var existing = await GetTransactionById(transaction.Id);
                if (existing is null)
                {
                    string message = "Selected transaction does not exists.";
                    return new ErrorResponse { Errors = [message], Message = message, Succeeded = false };
                }

                var account = await _accountService.GetAccountById(transaction.AccountId);
                if (account is null) {
                    string message = "You cannot update a transaction for an account that was closed or deleted";
                    return new ErrorResponse { Errors = [message], Message = "Selected account does not exist", Succeeded = false };
                }
                account.Balance = existing.Total + transaction.Total;

                var entity = _mapper.Map<Transaction>(transaction);
                entity = await _transactionRepository.Update(entity);
              
                await _accountService.Update(account);
                await _transactionManager.CommitTransactionAsync();
                return new Response<TransactionDto>
                {
                    Data = _mapper.Map<TransactionDto>(entity),
                    Succeeded = existing.Total != entity.Amount,
                    Message = "You have successfully updated your balance"
                };
            }
            catch(Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                _logger.LogError(ex, "An unexpected error occured while updating a transaction. Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
