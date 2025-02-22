


namespace Va.Developer.Assessment.Api.Endpoints
{
    public class PersonsController(IPersonService personService, IValidator<PersonDto> personValidator) : ApiBaseController
    {
        private readonly IPersonService _personService = personService;
        private readonly IValidator<PersonDto> _validator = personValidator;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PersonDto person)
        {
            var result = await _validator.ValidateAsync(person);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                return Conflict(new ErrorResponse { Errors = errors, Succeeded = !result.IsValid });
            }
            person.IdNo = Regex.Replace(person.IdNo, @"(?<=^\d{2})\d{2}(?=\d{6})", "XX").Insert(10, "XX");
            var response = await _personService.Add(person);
            if (!response.Succeeded)
            {
                Log.Error("An error occured while adding a user. Error: {Message}",response.Message);
                return BadRequest(response);
            }
            Log.Information(response.Message, person.FirstName, person.LastName);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var people = await _personService.Get();
            Log.Information("{Rows} users with {Account} accounts were successfully retrieved", people.Count(), people.Select(p => p.Accounts).Count());
            return Ok(new Response<IEnumerable<PersonDto>>() { Data = people, Succeeded = people.Any() });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person is null)
            {
                string message = "Selected person does not exists";
                return BadRequest(new ErrorResponse { Errors = [message], Message = message });
            }
            return Ok(new Response<PersonDto> { Data = person, Succeeded = true });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person is null)
            {
                string message = "Selected person does not exists";
                return BadRequest(new ErrorResponse { Errors = [message], Message = message });
            }
            await _personService.Update(person);
            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> Update(int id, [FromBody] JsonPatchDocument<PersonDto> document)
        {
            var person = await _personService.GetPersonById(id);
            if (person is null)
            {
                string message = "Selected person does not exists";
                return BadRequest(new ErrorResponse { Errors = [message], Message = message });
            }

            document.ApplyTo(person);
            var result = await _validator.ValidateAsync(person);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                return Conflict(new ErrorResponse { Errors = errors, Succeeded = !result.IsValid });
            }
            person = await _personService.Update(person);
            Log.Information("You have successfully updated {@Name} {@Surname} person", person.FirstName, person.LastName);
            return Ok(new Response<PersonDto> { Data = person, Message = "You have successfully added a person", Succeeded = person.Id > 0 });
        }
    }
}