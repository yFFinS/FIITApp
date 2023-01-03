using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

public record QuantityUnitRepositoryOptions(string FilePath, bool CacheUnits) : IOptions;

public class QuantityUnitRepository : IQuantityUnitRepository
{
    private readonly ILogger<QuantityUnitRepository> _logger;
    private readonly QuantityUnitRepositoryOptions _options;

    private List<QuantityUnit>? _cachedUnits;

    public QuantityUnitRepository(ILogger<QuantityUnitRepository> logger, QuantityUnitRepositoryOptions options)
    {
        _logger = logger;
        _options = options;
    }

    private static QuantityUnit ReadUnit(IReaderRow reader)
    {
        var name = reader.GetField<string>(0);
        var abbreviation = reader.GetField<string>(1);

        double? grams = reader.TryGetField<double>(2, out var gramEquivalent) ? gramEquivalent : null;
        double? milliliters = reader.TryGetField<double>(3, out var milliliterEquivalent) ? milliliterEquivalent : null;

        return new QuantityUnit(name!, abbreviation!, grams, milliliters);
    }

    private List<QuantityUnit> ReadUnits()
    {
        if (!File.Exists(_options.FilePath))
        {
            _logger.LogError("File {OptionsFilePath} not found", _options.FilePath);
            return new List<QuantityUnit>();
        }

        var units = new List<QuantityUnit>();

        using var streamReader = new StreamReader(_options.FilePath);
        using var reader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

        while (reader.Read())
        {
            var unit = ReadUnit(reader);
            units.Add(unit);
        }

        return units;
    }

    public IReadOnlyList<QuantityUnit> GetAllUnits()
    {
        if (_options.CacheUnits && _cachedUnits is not null)
        {
            _logger.LogDebug("Returning cached units");
            return _cachedUnits;
        }

        _logger.LogDebug("Reading units from file {OptionsFilePath}", _options.FilePath);
        var units = ReadUnits();

        if (_options.CacheUnits)
        {
            _logger.LogDebug("Caching units");
            _cachedUnits = units;
        }

        return units;
    }
}