using Avalonia.Media.Imaging;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;

namespace Recipes.Application.Services;

public class CachingImageLoader : IImageLoader
{
    private readonly ILogger<CachingImageLoader> _logger;
    private readonly int _cacheSize;

    private readonly HttpClient _httpClient = new();
    private readonly Dictionary<Uri, Bitmap> _cache = new();

    public CachingImageLoader(ILogger<CachingImageLoader> logger, int cacheSize = 100)
    {
        _logger = logger;
        _cacheSize = cacheSize;
    }

    private void SaveToCache(Uri uri, Bitmap bitmap)
    {
        if (_cache.Count >= _cacheSize)
        {
            _cache.Remove(_cache.Keys.First());
        }

        _cache.Add(uri, bitmap);
    }

    public async Task<Bitmap> LoadImage(Uri imageUri)
    {
        if (_cache.TryGetValue(imageUri, out var image))
        {
            _logger.LogDebug("Image {ImageUri} found in cache", imageUri);
            return image;
        }

        _logger.LogDebug("Image {ImageUri} not found in cache, loading from Uri", imageUri);

        await using var imageStream = await _httpClient.GetStreamAsync(imageUri);
        image = new Bitmap(imageStream);

        SaveToCache(imageUri, image);

        return image;
    }
}