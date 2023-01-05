using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Recipes.Application.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation;

public class IdToProductNameConverter : IMultiValueConverter
{
    public static readonly IdToProductNameConverter Instance = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is not EntityId id)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (values[1] is not IProductRepository repository)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(string)))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        return repository.GetProductByIdAsync(id);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}