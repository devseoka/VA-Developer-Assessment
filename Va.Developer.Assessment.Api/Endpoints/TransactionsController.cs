namespace Va.Developer.Assessment.Api.Endpoints
{
    public class TransactionsController(IValidator<TransactionDto> validator, ITransactionService transactionService) : ApiBaseController
    {
        private readonly IValidator<TransactionDto> _validator = validator;
        private readonly ITransactionService _transactionService = transactionService;
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]TransactionDto transaction){
            var result = await _validator.ValidateAsync(transaction);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                string message = "An validation errors occured while adding a transaction";
                return Conflict(new ErrorResponse { Errors = errors, Message = message, Succeeded = false });
            }
            var response = await  _transactionService.Add(transaction);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var transaction = await _transactionService.GetTransactionById(code: id);
            if(transaction == null)
            {
                var message = "Selected transaction does not exist.";
                return BadRequest(new ErrorResponse { Errors = [message], Message = message, Succeeded = false });
            }
            return Ok(new Response<TransactionDto>
            {
                Data = transaction,
                Succeeded = transaction is not null
            });
        }
    }
}