namespace Va.Developer.Assessment.Application.Validators;

public class TransactionValidator : AbstractValidator<TransactionDto>
{
    public TransactionValidator()
    {
        RuleFor(x => x.OrderedDate)
         .LessThanOrEqualTo(DateTime.Now)
         .WithMessage("The transaction date can never be in the future");
        RuleFor(x => x.Total)
         .NotEmpty()
         .WithMessage("Transaction amount is required")
         .NotEqual(0)
         .WithMessage("Transaction amount should not be equal to zero");
    }
}
