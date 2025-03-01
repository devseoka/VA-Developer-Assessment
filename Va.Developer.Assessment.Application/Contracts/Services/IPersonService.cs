namespace Va.Developer.Assessment.Application.Contracts.Services
{
    public interface IPersonService
    {
        IQueryable<PersonDto> People { get;  }
        Task<IEnumerable<PersonDto>> Get();
        Task<IEnumerable<PersonDto>> Get(int pageNumber, int pageSize);
        Task<IResponse> Add(PersonDto person);
        Task Delete(PersonDto person);
        Task<PersonDto> Update(PersonDto person);
        Task<PersonDto> GetPersonById(int id);
        Task<PersonDto> GetPersonByIdNumber(string id);
    }
}