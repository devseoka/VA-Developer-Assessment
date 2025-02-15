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

        public async Task<PersonDto> Add(PersonDto person)
        {
            var entity = _mapper.Map<Person>(person);
            entity = await _personRepostiory.Add(entity);
            return _mapper.Map<PersonDto>(entity);
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

        public async Task<IEnumerable<PersonDto>> Get(int page, int size)
        {
            return await People
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToListAsync();
        }

        public async Task<PersonDto> GetPersonById(int code)
        {
            return await People.FirstOrDefaultAsync(p => p.Id == code);
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