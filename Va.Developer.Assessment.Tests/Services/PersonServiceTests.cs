using Va.Developer.Assessment.Application.Dto;
using Va.Developer.Assessment.Application.Response;

namespace Va.Developer.Assessment.Tests.Services
{
    public class PersonServiceTests(WebAssessmentFactory web) : AssessmentBaseTests(web)
    {
        private readonly PersonService _personService = new(web.PersonRepostiory, web.Mapper);

        [Fact(DisplayName = "Get People Returns People")]
        public async Task GetPeople_Returns_People()
        {
            var people = await _personService.Get();
            Assert.NotEmpty(people);
            Assert.True(people.Count() > 49);
        }
        [Fact(DisplayName = "Get People By Id Returns Person")]
        public async Task GetPeople_Returns_Person()
        {
            var person = await _personService.GetPersonById(id: 25);
            Assert.NotNull(person);
            Assert.Equal("Ramalepe", person.LastName, ignoreCase: true);
        }
        [Fact(DisplayName = "Get People By Id Returns Null")]
        public async Task GetPeople_Returns_Null()
        {
            var person = await _personService.GetPersonById(id: 89);
            Assert.Null(person);
        }
        [Fact(DisplayName = "Get People By Id Returns Success Response")]
        public async Task Add_Returns_SuccessResponse()
        {
            var person = new PersonDto { LastName = "Agents", FirstName = "Virtual", IdNo = "890322895591" };
            Response<PersonDto> response = (await _personService.Add(person)) as Response<PersonDto>;
            Assert.True(response.Succeeded);
            Assert.True(response.Data.Id > 50);
            Assert.Equal($"{person.FirstName} {person.LastName} was successfully added", response.Message);
        }
        [Fact(DisplayName = "Get People By Id Returns Id Already Exists Error Message")]
        public async Task Add_Returns_IdAlreadyExistErrorMessage()
        {
            var person = new PersonDto { LastName = "Agents", FirstName = "Virtual", IdNo = "44XX0801450XX" };
            var response = (await _personService.Add(person)) as ErrorResponse;
            Assert.False(response.Succeeded);
            Assert.Equal($"Id number already exists", response.Message);
        }
        [Fact(DisplayName = "Update Person  Returns Success Response")]
        public async Task UpdatePerson_Returns_SuccesssResponse()
        {
            var person = new PersonDto { LastName = "Agents", FirstName = "Virtual", IdNo = "44XX0801450XX" };
            var response = (await _personService.Add(person)) as Response<PersonDto>;

            Assert.True(response.Succeeded);
            Assert.True(response.Data.Id > 50);

            person.LastName = "The Virtual Agents";
            person = await _personService.Update(person) ;

            Assert.Equal("The Virtual Agents", person.LastName);
        }
        [Fact(DisplayName = "Delete Person  Returns Success Response")]
        public async Task Delete_Returns_SuccesssResponse()
        {
            var person = new PersonDto { LastName = $"Agents {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", FirstName = "Virtual", IdNo = "44XX0801450XX" };
            var response = (await _personService.Add(person)) as Response<PersonDto>;

            Assert.True(response.Succeeded);
            Assert.True(response.Data.Id > 50);

            await _personService.Delete(person) ;

            person = await _personService.GetPersonById(person.Id);
            Assert.Null(person);
        }
    }
}
