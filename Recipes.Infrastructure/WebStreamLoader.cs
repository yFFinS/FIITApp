namespace Recipes.Infrastructure;

public static class WebStreamLoader
{
    private static readonly HttpClient HttpClient = new();
    
    public static async Task<MemoryStream> LoadAsync(Uri url)
    {
        await using var httpStream = await HttpClient.GetStreamAsync(url);

        var memoryStream = new MemoryStream();
        await httpStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }
}