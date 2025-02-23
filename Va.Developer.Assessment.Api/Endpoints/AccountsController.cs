using Microsoft.AspNetCore.JsonPatch;

namespace Va.Developer.Assessment.Api.Endpoints
{
    public class AccountsController(IAccountService accountService) : ApiBaseController
    {
        private readonly IAccountService _accountService = accountService;
        private AccountDto Account { get; set; }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AccountDto account)
        {

            var response = await _accountService.Add(account);
            if (!response.Succeeded)
            {
                var error = response as ErrorResponse;
                error.Message = "An unexpected error occured while adding an account";
                Log.Warning(". {Message}. \r\n Errors: {@Errors} ", response.Message, error.Errors);
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> Get([FromRoute] int code)
        {
            var account = await _accountService.GetAccountById(code);
            if (account is null)
            {
                var message = "Selected account does not exists";
                var response = new ErrorResponse { Errors = [message], Message = message };
                return BadRequest(response);
            }
            return Ok(new Response<AccountDto> { Succeeded = true, Data = account });
        }
        [HttpPatch("{code}")]
        public async Task<IActionResult> Update([FromRoute] int code, [FromBody]AccountDto account)
        {
            Account = await _accountService.GetAccountById(code);
            if (Account is null)
            {
                var message = "Selected account does not exists";
                var result = new ErrorResponse { Errors = [message], Message = message };
                return BadRequest(result);
            }
            if (Account.Balance != account.Balance)
            {
                var message = "You are not allowed to update account balance";
                var result = new ErrorResponse { Errors = [message], Message = message };
                return BadRequest(result);
            }
            var response = await _accountService.Update(account);
            return Ok(response);
        }
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete([FromRoute] int code)
        {
            var account = await _accountService.GetAccountById(code);
            if (account is null)
            {
                var message = "Selected account does not exists";
                var result = new ErrorResponse { Errors = [message], Message = message };
                return BadRequest(result);
            }
            var response = await _accountService.Delete(account);
            if (!response.Succeeded)
            {
                response.Message = "An unexpected error occured while deleting an account";
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}