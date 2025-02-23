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
         .Equal(0)
         .WithMessage("Transaction should not be less or equal to zero");
    }
}
