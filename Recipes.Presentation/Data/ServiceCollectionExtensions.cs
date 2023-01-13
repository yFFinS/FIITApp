﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Recipes.Presentation.DataTypes;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFactory<TImplementation>(this IServiceCollection services)
        where TImplementation : notnull =>
        services.AddSingleton<Func<TImplementation>>(x => x.GetRequiredService<TImplementation>);
}