namespace Recipes.Infrastructure.DataBase;

public record DatabasePaths(string ProductsPath, string RecipesPath, string CustomRecipesPath);

public class DatabasePathsProvider
{
    private readonly DatabasePaths _paths;

    public DatabasePathsProvider(DatabasePaths paths)
    {
        _paths = paths;
    }

    public string GetAppPath()
    {
        var path = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RecipeKeeper");

        Directory.CreateDirectory(path);

        return path;
    }

    public string GetDatabasePath(DatabaseName databaseName)
    {
        var appPath = GetAppPath();
        return databaseName switch
        {
            DatabaseName.Products => Path.Join(appPath, _paths.ProductsPath),
            DatabaseName.Recipes => Path.Join(appPath, _paths.RecipesPath),
            DatabaseName.CustomRecipes => Path.Join(appPath, _paths.CustomRecipesPath),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseName), databaseName, null)
        };
    }
}