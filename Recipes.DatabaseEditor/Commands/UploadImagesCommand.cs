using Recipes.Application.Interfaces;
using Recipes.Domain.Interfaces;
using Recipes.Infrastructure.DataBase;

namespace Recipes.DatabaseEditor.Commands;

public class UploadImagesCommand : Command
{
    private readonly TextWriter _output;
    private readonly IProductRepository _productRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly FtpService _ftpService;

    public UploadImagesCommand(TextWriter output, IProductRepository productRepository,
        IRecipeRepository recipeRepository, FtpService ftpService) :
        base(new[] { "upload", "images" },
            "загружает отсутствующие изображения продуктов и рецептов на ftp сервер")
    {
        _output = output;
        _productRepository = productRepository;
        _recipeRepository = recipeRepository;
        _ftpService = ftpService;
    }

    public override void Execute(string[] args)
    {
        _output.WriteLine("Загрузка изображений продуктов и рецептов");
        var products = _productRepository.GetAllProducts();

        var uploadedCount = 0;
        var totalCount = 0;

        async Task UploadTask(Uri uri)
        {
            await _ftpService.UploadImage(uri);
            uploadedCount++;
            if (uploadedCount % 10 == 0)
            {
                await _output.WriteLineAsync($"Загружено {uploadedCount}/{totalCount}");
            }
        }

        var uris = new List<Uri>();
        foreach (var product in products
                     .Where(product => product.ImageUrl != null))
        {
            uris.Add(product.ImageUrl!);
            totalCount++;
        }

        var recipes = _recipeRepository.GetAllRecipes(onlyGlobal: true);
        foreach (var recipe in recipes
                     .Where(r => r.ImageUrl != null))
        {
            uris.Add(recipe.ImageUrl!);
            totalCount++;
        }

        foreach (var uri in uris)
        {
            try
            {
                UploadTask(uri).Wait();
            }
            catch (Exception e)
            {
                _output.WriteLine(e.Message);
            }
        }

        _output.WriteLine("Загрузка изображений завершена");
    }
}