using Avalonia.Data;
using Avalonia.Data.Converters;
using Recipes.Domain.Entities.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Recipes.Presentation.Converters;

public class ProductCheckedConverter : IMultiValueConverter
{
    public static readonly ProductCheckedConverter Instance = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is not HashSet<Product> check)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (values[1] is not Product product)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return check.Contains(product);
    }
}