namespace Va.Developer.Assessment.Application.Dto
{
    public class AccountDto : DtoModelBase
    {
        public int UserId { get; set; }
        public string AccountNo { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}