namespace Va.Developer.Assessment.Domain.ValueObject
{
    public class Money : IEquatable<Money>
    {

        public decimal Amount { get; }
        public string Currency { get; } = "R";
        public Money(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            }
            Amount = amount;
        }
        public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
        public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
        public static Money operator *(Money a, decimal multiplier) => new(a.Amount * multiplier);
        public static Money operator /(Money a, decimal divisor) => new(a.Amount / divisor);
        public bool Equals(Money other) => other is not null && Amount == other.Amount;
        public override bool Equals(object obj) => Equals(obj as Money);
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
        public override string ToString() => $"{Currency} {Amount:N2}";
    }
}