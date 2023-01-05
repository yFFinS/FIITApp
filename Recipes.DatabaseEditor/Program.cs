using Microsoft.Extensions.DependencyInjection;
using Recipes.DatabaseEditor;
using Recipes.DatabaseEditor.Commands;
using Recipes.Domain.Interfaces;
using Recipes.Infrastructure;

var services = Bootstrap.ConfigureServices();

services.AddSingleton(Console.In);
services.AddSingleton(Console.Out);

services.AddSingleton(new ProductNameUnifierOptions("product_mapping.csv"));
services.AddSingleton<IProductNameUnifier, ProductNameUnifier>();

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