using Va.Developer.Assessment.Domain.Contracts.Repository;
using Va.Developer.Assessment.Domain.Models;
using Va.Developer.Assessment.Infrastructure.Persistence.Context;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Repositories
{
    public class PersonRepository(VaDeveloperContext context) : IPersonRepostiory
    {
        private readonly VaDeveloperContext _context = context;
        public IQueryable<Person> People => _context.People;

        public async Task<Person> Add(Person person)
        {
            var entry = _context.People.Add(person);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task Delete(Person person)
        {
            var entry = _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }

        public async Task<Person> Update(Person person)
        {
             var entry = _context.People.Update(person);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}