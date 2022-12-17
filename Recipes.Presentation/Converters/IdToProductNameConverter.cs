using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Recipes.Application.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation;

public class IdToProductNameConverter : IValueConverter
{
    public static readonly IdToProductNameConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not EntityId id)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (parameter is not IProductRepository repository)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(string)))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        return repository.GetProductByIdAsync(id).Result.Name;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}