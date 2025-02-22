using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Application.Services
{
    public class PersonService(IPersonRepostiory personRepostiory, IMapper mapper) : IPersonService
    {
        private readonly IPersonRepostiory _personRepostiory = personRepostiory;
        private readonly IMapper _mapper = mapper;

        public IQueryable<PersonDto> People =>
        _personRepostiory
                    .People.Include(p => p.Accounts)
                    .ProjectTo<PersonDto>(_mapper.ConfigurationProvider);

        public async Task<IResponse> Add(PersonDto person)
        {
            var entity = _mapper.Map<Person>(person);
            if(await People.AnyAsync(p => p.IdNo == person.IdNo))
            {
                string message = "Id number already exists";
                return new ErrorResponse { Message = message, Succeeded = false, Errors = [message] };
            }
            entity = await _personRepostiory.Add(entity);
            person = _mapper.Map<PersonDto>(entity);
            return new Response<PersonDto> 
            { 
                Data = person, 
                Message = $"{person.FirstName} {person.LastName} was successfully added",
                Succeeded = person.Id  > 0
            };
        }

        public async Task Delete(PersonDto person)
        {
            var entity = _mapper.Map<Person>(person);
            await _personRepostiory.Delete(entity);
        }

        public async Task<IEnumerable<PersonDto>> Get()
        {
            return await People.ToListAsync();
        }

        public async Task<IEnumerable<PersonDto>> Get(int pageNumber, int pageSize)
        {
            return await People
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public async Task<PersonDto> GetPersonById(int id)
        {
            return await People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PersonDto> GetPersonByIdNumber(string id)
        {
            return await People.FirstOrDefaultAsync(p => p.IdNo == id);
        }

        public async Task<PersonDto> Update(PersonDto person)
        {
            var entity = _mapper.Map<Person>(person);
            entity = await _personRepostiory.Update(entity);
            return _mapper.Map<PersonDto>(entity);
        }
    }
}