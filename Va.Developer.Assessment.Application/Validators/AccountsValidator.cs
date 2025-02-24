namespace Va.Developer.Assessment.Application.Validators;

public class AccountsValidator : AbstractValidator<AccountDto>
{
    public AccountsValidator()
    {
        RuleFor(a => a.AccountNo)
            .NotEmpty()
            .WithMessage("Account number is required");
        RuleFor(a => a.Balance)
            .NotEmpty()
            .WithMessage("Account balance is required");
        RuleFor(a => a.UserId)
            .NotEmpty()
            .WithMessage("Account number should belong to a user");
    }
}
