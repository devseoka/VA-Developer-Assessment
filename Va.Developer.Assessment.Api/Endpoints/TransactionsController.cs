namespace Va.Developer.Assessment.Api.Endpoints
{
    public class TransactionsController : ApiBaseController
    {
        [HttpPost]
        public Task<IActionResult> Add([FromBody]TransactionDto transaction){
            throw new NotImplementedException();
        }
    }
}