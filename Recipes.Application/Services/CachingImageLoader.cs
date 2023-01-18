using Avalonia.Media.Imaging;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Domain.Interfaces;

namespace Recipes.Application.Services;

public record CachingImageLoaderOptions(int CacheSize = 100) : IOptions;

public class CachingImageLoader : IImageLoader
{
    private readonly ILogger<CachingImageLoader> _logger;
    private readonly CachingImageLoaderOptions _options;

    private readonly HttpClient _httpClient = new();
    private readonly Dictionary<Uri?, Bitmap> _cache = new();

    public CachingImageLoader(ILogger<CachingImageLoader> logger, CachingImageLoaderOptions options)
    {
        _logger = logger;
        _options = options;
    }

    private void SaveToCache(Uri? uri, Bitmap bitmap)
    {
        if (_cache.Count >= _options.CacheSize)
        {
            _cache.Remove(_cache.Keys.First());
        }

        _cache.Add(uri, bitmap);
    }

    public async Task<Bitmap> LoadImage(Uri? imageUri)
    {
        if (_cache.TryGetValue(imageUri, out var image))
        {
            _logger.LogDebug("Image {ImageUri} found in cache", imageUri);
            return image;
        }

        _logger.LogDebug("Image {ImageUri} not found in cache, loading from Uri", imageUri);

        await using var httpStream = await _httpClient.GetStreamAsync(imageUri);

        using var memoryStream = new MemoryStream();
        await httpStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        image = new Bitmap(memoryStream);

        SaveToCache(imageUri, image);

        return image;
    }
}