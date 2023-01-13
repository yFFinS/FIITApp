using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace Recipes.Presentation.Converters;

public class MainMenuAlignmentConverter : IValueConverter
{
    public static readonly MainMenuAlignmentConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(targetType != typeof(HorizontalAlignment))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        return value != null ? HorizontalAlignment.Left : HorizontalAlignment.Center;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}