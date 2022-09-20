using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace Recipes.Shared;

public static class GuardClauseExtensions
{
    public static double NegativeOrInvalid(this IGuardClause guardClause,
        double input, [InvokerParameterName] string? parameterName = null)
    {
        input = guardClause.Negative(input, parameterName);
        
        if (double.IsNaN(input))
        {
            throw new ArgumentException("Value cannot be NaN.", parameterName);
        }

        if (double.IsInfinity(input))
        {
            throw new ArgumentException("Value cannot be infinite.", parameterName);
        }

        return input;
    }
}