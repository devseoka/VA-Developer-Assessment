namespace Va.Developer.Assessment.Application.Dto
{
    public class PersonDto : DtoModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNo { get; set; }
        public List<AccountDto> Accounts {get; set;}
    }
}