using System.Net;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Logging;
using Recipes.Application.Interfaces;
using Recipes.Infrastructure.DataBase;

namespace Recipes.Infrastructure;

public class CachingImageLoader : IImageLoader
{
    private readonly ILogger<CachingImageLoader> _logger;
    private readonly FtpService _ftpService;
    private readonly CachingImageLoaderOptions _options;

    private readonly Dictionary<Uri, Bitmap> _cache = new();

    public CachingImageLoader(ILogger<CachingImageLoader> logger, FtpService ftpService,
        CachingImageLoaderOptions options)
    {
        _logger = logger;
        _ftpService = ftpService;
        _options = options;
    }

    private void SaveToCache(Uri uri, Bitmap bitmap)
    {
        if (_cache.Count >= _options.CacheSize)
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

        MemoryStream stream;

        try
        {
            var imageData = await _ftpService.DownloadImage(imageUri);
            stream = new MemoryStream(imageData);
        }
        catch (WebException)
        {
            _logger.LogWarning("Image {ImageUri} not found on server. Loading directly from Uri", imageUri);
            stream = await WebStreamLoader.LoadAsync(imageUri);
        }

        using (stream)
        {
            image = new Bitmap(stream);
            SaveToCache(imageUri, image);
            return image;
        }
    }
}