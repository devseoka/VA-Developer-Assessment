using Docker.DotNet.Models;
using Va.Developer.Assessment.Application.Response;

namespace Va.Developer.Assessment.Tests.Services
{
    public class TransactionServiceTests(WebAssessmentFactory web) : AssessmentBaseTests(web)
    {
        private readonly TransactionService _transactionService = new
            (
            web.AccountService,
            web.TransactionManager, 
            web.TransactionRepository, 
            web.Mapper,
            web.Logger);

        private readonly IAccountService _accountService = web.AccountService;

        [Fact(DisplayName = "Add Transaction Returns Success Response")]
        public async Task AddTransaction_Returns_SuccessResponse()
        {
            var account = await _accountService.GetAccountById(12);
            Assert.NotNull(account);

            var response = await _transactionService.Add(new()
            {
                Description = "Virtual Agent Salary",
                AccountId = account.Id,
                OrderedDate = DateTime.UtcNow,
                Total = 57000
            }) as Response<TransactionDto>;
            Assert.NotNull(response.Data);
            Assert.Equal(DateTime.Now, response.Data.ProcessedDate, TimeSpan.FromHours(5));
            Assert.Equal($"You have successfully added a credit transaction.", response.Message);

        }

        [Fact(DisplayName = "Add Transaction Returns Closed Account Error")]
        public async Task AddTransaction_Returns_ClosedAccountError()
        {
            var response = await _transactionService.Add(new()
            {
                Description = "Virtual Agent Salary",
                AccountId = 798,
                OrderedDate = DateTime.UtcNow,
                Total = 57000
            }) as ErrorResponse;
           
            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Equal($"Selected account for the transaction is closed or deleted", response.Message);

        }
        [Fact(DisplayName = "Add Transaction Returns Zero Transaction Amount Error")]
        public async Task AddTransaction_Returns_ZeroTransactionAmountError()
        {
            var account = await _accountService.GetAccountById(7);
            Assert.NotNull(account);

            var response = await _transactionService.Add(new()
            {
                Description = "Virtual Agent Salary",
                AccountId = account.Id,
                OrderedDate = DateTime.UtcNow,
                Total = 0
            }) as ErrorResponse;

            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Equal($"The transaction amount can never be zero.", response.Message);

        }
        [Fact(DisplayName = "Update Transaction Returns Success Response")]
        public async Task UpdateTransaction_Returns_SuccessResponse()
        {
            var transaction = await _transactionService.GetTransactionById(12);
            Assert.NotNull(transaction);
            transaction.Description = "Virtual Agent Salary";
            transaction.Total = 25700;
           
            var response = await _transactionService.Update(transaction) as Response<TransactionDto>;
            Assert.NotNull(response.Data);
            Assert.Equal(DateTime.Now, response.Data.ProcessedDate, TimeSpan.FromHours(5));
            Assert.Equal($"You have successfully updated your transaction", response.Message);

        }
        [Fact(DisplayName = "Update Transaction Returns Transaction Does Not Exist Error")]
        public async Task UpdateTransaction_Returns_TransactionDoesNotExistError()
        {
            var transaction = await _transactionService.GetTransactionById(12);
            Assert.NotNull (transaction);
            transaction.Id = 0;
            var response = await _transactionService.Update(transaction) as ErrorResponse;
            
            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Equal($"Selected transaction does not exists.", response.Message);
        }
        [Fact(DisplayName = "Update Transaction Returns Close or Deleted Account Error")]
        public async Task UpdateTransaction_Returns_CloseOrDeletedAccountError()
        {
            var transaction = await _transactionService.GetTransactionById(12);
            Assert.NotNull(transaction);
            transaction.AccountId = 800;
            var response = await _transactionService.Update(transaction) as ErrorResponse;

            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Contains($"Selected account does not exist", response.Message);
            Assert.Contains("You cannot update a transaction for an account that was closed or deleted", response.Errors);
        }

    }
}
