using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Interfaces;
using Recipes.DatabaseEditor;
using Recipes.DatabaseEditor.Commands;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using Recipes.Infrastructure;

var services = Bootstrap.ConfigureServices();

services.AddSingleton<IProductRepository>(new Mock1());
services.AddSingleton<IRecipeRepository>(new Mock2());

services.AddSingleton(Console.In);
services.AddSingleton(Console.Out);

services.AddSingleton<ProductParser>();
services.AddSingleton<RecipeParser>();
services.AddSingleton<Interpreter>();

// Register commands
services.AddSingleton<Command, AddProductCommand>();
services.AddSingleton<Command, AddRecipeCommand>();
services.AddSingleton<Command, RemoveProductsCommand>();
services.AddSingleton<Command, RemoveRecipeCommand>();
services.AddSingleton<Command, HelpCommand>();

services.AddSingleton<IReadOnlyList<Command>>(sp => sp.GetServices<Command>().ToList());
services.AddSingleton(sp => new Lazy<IReadOnlyList<Command>>(() => sp.GetServices<Command>().ToList()));

IServiceProvider serviceProvider = services.BuildServiceProvider();

var interpreter = serviceProvider.GetRequiredService<Interpreter>();
interpreter.Run();


public class Mock1 : IProductRepository
{
    public Task<List<Product>> GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetProductByIdAsync(EntityId productId)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetProductByNameAsync(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> GetProductsByPrefixAsync(string productNamePrefix)
    {
        throw new NotImplementedException();
    }

    public Task AddProductsAsync(IEnumerable<Product> products)
    {
        throw new NotImplementedException();
    }

    public Task RemoveProductsByIdAsync(IEnumerable<EntityId> products)
    {
        throw new NotImplementedException();
    }
}

public class Mock2 : IRecipeRepository
{
    public Task<List<Recipe>> GetRecipesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Recipe?> GetRecipeByIdAsync(EntityId recipeId)
    {
        throw new NotImplementedException();
    }

    public Task<Recipe?> GetRecipeByNameAsync(string recipeName)
    {
        throw new NotImplementedException();
    }

    public Task AddRecipesAsync(IEnumerable<Recipe> recipes)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRecipesByIdAsync(IEnumerable<EntityId> recipeIds)
    {
        throw new NotImplementedException();
    }
}