using Va.Developer.Assessment.Application.Response;

namespace Va.Developer.Assessment.Tests.Services
{
    public class AccountServiceTests(WebAssessmentFactory web) : AssessmentBaseTests(web)
    {
        private readonly AccountService _accountService = new(web.AccountValidator, web.Mapper, web.PersonService, web.AccountRepository);
        private readonly IPersonService _personService = web.PersonService;

        [Fact(DisplayName = "Add Account Returns Success Response")]
        public async Task Add_Returns_SuccessResponse()
        {
            var person = await _personService.GetPersonById(20);
            Assert.NotNull(person);

            var response = await _accountService.Add(new AccountDto
            {
                AccountNo = "9520250220",
                Balance = 1,
                UserId = person.Id
            }) as Response<AccountDto>;

            Assert.NotNull(response.Data);
            Assert.True(response.Succeeded);
            Assert.Equal($"You have successfully added {person.FirstName} {person.LastName}'s account", response.Message);
        }
        [Theory(DisplayName = "Add Account Returns Validation Errrors")]
        [InlineData("", 0, 0)]
        [InlineData("", 4, 20)]
        [InlineData("9505000022", 0, 20)]

        public async Task Add_Returns_ValidationErrors(string accountNo, decimal balance, int id)
        {
            var response = await _accountService.Add(new AccountDto
            {
                AccountNo = accountNo,
                Balance = balance,
                UserId = id
            }) as ErrorResponse;

            Assert.NotEmpty(response.Errors);
            Assert.False(response.Succeeded);
            if (string.IsNullOrWhiteSpace(accountNo))
            {
                Assert.Contains("Account number is required", response.Errors);
            }
            else if(balance == 0)
            {
                Assert.Contains("Account balance is required", response.Errors);
            }
        }
        [Fact(DisplayName = "Add Account Returns Selected Person Does Not Exis")]
        public async Task AddAccount_Returns_SelectedPersonDoesNotExist()
        {
            var response = await _accountService.Add(new AccountDto()
            {
                AccountNo = "9520250220",
                Balance = 1,
                UserId = 500,
            }) as ErrorResponse;
            Assert.NotEmpty(response.Errors);
            Assert.False(response.Succeeded);
            Assert.Equal("The person you are attempting to add an account for does not exists", response.Message);
        }
        [Fact(DisplayName = "Add Account Returns Account Number Already Exists.")]
        public async Task AddAccount_Returns_AccountNumberAlreadyExists()
        {
            var account = await _accountService.GetAccountById(10);
            Assert.NotNull(account);

            var response = await _accountService.Add(new AccountDto()
            {
                AccountNo = account.AccountNo,
                Balance = 1,
                UserId = account.UserId,
            }) as ErrorResponse;
            Assert.NotEmpty(response.Errors);
            Assert.False(response.Succeeded);
            Assert.Equal("Account number already exists.", response.Errors.First());
        }
        [Fact(DisplayName = "Update Account Returns Success Response")]
        public async Task UpdateAccount_Returns_SuccessResponse()
        {
            var account = await _accountService.GetAccountById(10);
            Assert.NotNull(account);
            account.AccountNo = "850222890";
            var response = await _accountService.Update(account) as Response<AccountDto>;
            Assert.True(response.Succeeded);
            Assert.Equal("850222890", account.AccountNo);
            Assert.Equal("Account was successfully updated", response.Message);
        }
        [Fact(DisplayName = "Update Account Returns Account Does Not Exist Error")]
        public async Task UpdateAccount_Returns_AccountDoesNotExists()
        {
            var account = await _accountService.GetAccountById(10);
            Assert.NotNull(account);
            account.Id = 850;
            account.AccountNo = "850222890";
            var response = await _accountService.Update(account) as ErrorResponse;
            Assert.False(response.Succeeded);
            Assert.Contains("Selected account does not exists", response.Errors);
        }
        [Fact(DisplayName = "Delete Account Returns Success Response")]
        public async Task DeleteAccount_Returns_SuccessResponse()
        {
            var response = await _accountService.Add(new AccountDto
            {
                AccountNo = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                Balance = 50,
                UserId = 10,
            }) as Response<AccountDto>;
            Assert.NotNull(response.Data);
            response = await _accountService.Delete
                (response.Data) as Response<AccountDto>;            
            var account = await _accountService.GetAccountById(response.Data.Id);
            Assert.Null(account);
        }
        [Fact(DisplayName = "Delete Account Returns Account Does Not Exists")]
        public async Task DeleteAccount_Returns_AccountDoesNotExists()
        {
            var account = await _accountService.GetAccountById(10);
            Assert.NotNull(account);
            
            account.Balance = 0;
            account.Id = 298;
            
            var response = await _accountService.Delete
                (account) as ErrorResponse;
            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Contains("Selected account does not exists", response.Errors);
        }
        [Fact(DisplayName = "Delete Account Returns Close or Delete Error")]
        public async Task DeleteAccount_Returns_CloseOrDeleteError()
        {
            var account = await _accountService.GetAccountById(10);
            Assert.NotNull(account);

            account.Balance = 100;
            account.Id = 298;

            var response = await _accountService.Delete
                (account) as ErrorResponse;
            Assert.False(response.Succeeded);
            Assert.NotEmpty(response.Errors);
            Assert.Contains("Account cannot be deleted or closed if balance is not zero", response.Errors);
        }

    }
}
