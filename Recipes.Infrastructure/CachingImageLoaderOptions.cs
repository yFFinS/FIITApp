using Recipes.Domain.Interfaces;

namespace Recipes.Infrastructure;

public record CachingImageLoaderOptions(int CacheSize = 100) : IOptions;