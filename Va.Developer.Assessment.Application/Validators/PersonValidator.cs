namespace Va.Developer.Assessment.Application.Validators
{
    public class PersonValidator : AbstractValidator<PersonDto>
    {
        public PersonValidator(){
            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MinimumLength(2)
                .WithMessage("First name should have more than one character");
            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MinimumLength(2)
                .WithMessage("Last name should have more than one character");
            RuleFor(p => p.IdNo)
                .NotEmpty()
                .WithMessage("Id number number is required")
                .Length(13)
                .WithMessage("Id number should have 13 digits");
        }
    }
}