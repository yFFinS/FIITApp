﻿using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Presentation.Interfaces;

namespace Recipes.Presentation.DataTypes;

public class ImageWrapper<T>
    where T : Recipe
{
    public ImageWrapper(T o, IImageLoader imageLoader)
    {
        Object = o;
        ImageLoader = imageLoader;
    }

    public T Object { get; set; }
    public IImageLoader ImageLoader { get; }
    public Task<Bitmap> Image => ImageLoader.LoadImage(Object.ImageUrl);

    public static implicit operator T(ImageWrapper<T> wrapper) => wrapper.Object;
}