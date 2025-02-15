using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Domain.Contracts.Repository;

public interface IPersonRepostiory
{
    IQueryable<Person> People { get; }
    Task<Person> Add(Person person);
    Task<Person> Update(Person person);
    Task Delete(Person person);
}