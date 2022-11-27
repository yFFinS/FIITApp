using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Presentation;

public class RecipeToImageConverter : IValueConverter
{
    public static readonly RecipeToImageConverter Instance = new ();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Recipe recipe)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (parameter is not IImageLoader loader)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(IImage)))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (recipe.ImageUrl != null)
            return loader.LoadImage(recipe.ImageUrl);

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}