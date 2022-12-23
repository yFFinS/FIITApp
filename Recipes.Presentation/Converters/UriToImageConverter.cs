using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation;

public class UriToImageConverter : IMultiValueConverter
{
    public static readonly UriToImageConverter Instance = new ();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is not Uri uri)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (values[1] is not IImageLoader loader)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(IImage)))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        return loader.LoadImage(uri).Result;
    }
}