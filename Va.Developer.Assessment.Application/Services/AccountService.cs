using Va.Developer.Assessment.Domain.Models;


namespace Va.Developer.Assessment.Application.Services;

public class AccountService(IValidator<AccountDto> validator, IMapper mapper, IPersonService personService, IAccountRepository accountRepository) : IAccountService
{
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<AccountDto> _accountValidator = validator;
    private readonly IPersonService _personService = personService;
    private readonly IAccountRepository _accountRepository = accountRepository;
    public IQueryable<AccountDto> Accounts =>
            _accountRepository
                .Accounts
                .Include(a => a.Transactions.OrderByDescending(t => t.CaptureDate))
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider);

    public async Task<IResponse> Add(AccountDto account)
    {
        var result = await _accountValidator.ValidateAsync(account);
        if (!result.IsValid)
        {
            var message = "A validation error(s) occured while adding account";
            var errors = result.Errors.Select(e => e.ErrorMessage);
            return new ErrorResponse { Errors = errors, Message = message };
        }
        var person = await _personService.GetPersonById(account.UserId);
        if (person is null)
        {
            var message = "The person you are attempting to add an account for does not exists";
            return new ErrorResponse { Errors = [message], Message = message };
        }
        if (await Accounts.AnyAsync(a => a.AccountNo == account.AccountNo))
        {
            var message = "Account number already exists.";
            return new ErrorResponse { Errors = [message], Message = message };
        }
        account.Balance = 0;
        var entity = await _accountRepository.Add(_mapper.Map<Account>(account));
        account = _mapper.Map<AccountDto>(entity);

        return new Response<AccountDto> { Data = account, Message = $"You have successfully added {person.FirstName} {person.LastName}'s account", Succeeded = true };
    }

    public async Task<IResponse> Delete(AccountDto account)
    {
        if (account.Balance < 1)
        {
            var message = "Account cannot be deleted or closed if balance is not zero";
            return new ErrorResponse { Errors = [message], Message = message };
        }
        await _accountRepository.Delete(_mapper.Map<Account>(account));
        return new Response<AccountDto> { Message = "You have uccessfully deleted account.", Succeeded = true };
    }

    public async Task<AccountDto> GetAccountById(int code)
    {
        return await Accounts.FirstOrDefaultAsync(a => a.Id == code);
    }

    public async Task<IEnumerable<AccountDto>> GetAccounts()
    {
        return await Accounts.ToListAsync();
    }

    public async Task<IEnumerable<AccountDto>> GetAccounts(int code)
    {
        return await Accounts.Where(a => a.UserId == code).ToListAsync();
    }

    public async Task<IResponse> Update(AccountDto account)
    {
        var entity = await _accountRepository.Update(_mapper.Map<Account>(account));
        account = _mapper.Map<AccountDto>(entity);
        return new Response<AccountDto> { Data = account, Message = "Account was successfully updated", Succeeded = true};
    }
}
