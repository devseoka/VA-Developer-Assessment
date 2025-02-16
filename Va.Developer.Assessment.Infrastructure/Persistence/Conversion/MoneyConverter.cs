namespace Va.Developer.Assessment.Infrastructure.Persistence.Conversion
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Va.Developer.Assessment.Domain.ValueObject;

    public class MoneyConverter(Expression<Func<Money, string>> convertToProviderExpression, Expression<Func<string, Money>> convertFromProviderExpression, ConverterMappingHints mappingHints = null) : ValueConverter<Money, string>(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }
}