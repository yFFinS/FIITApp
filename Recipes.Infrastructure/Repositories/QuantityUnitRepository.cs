using CsvHelper;
using Microsoft.Extensions.Logging;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using System.Globalization;

namespace Recipes.Infrastructure.Repositories;

public record QuantityUnitRepositoryOptions(string FilePath, bool CacheUnits) : IOptions;

public class QuantityUnitRepository : IQuantityUnitRepository
{
    private readonly ILogger<QuantityUnitRepository> _logger;
    private readonly QuantityUnitRepositoryOptions _options;

    private Dictionary<int, QuantityUnit>? _cachedUnits;

    public QuantityUnitRepository(ILogger<QuantityUnitRepository> logger, QuantityUnitRepositoryOptions options)
    {
        _logger = logger;
        _options = options;
    }

    private static (int Id, QuantityUnit Unit) ReadUnit(IReaderRow reader)
    {
        var id = reader.GetField<int>(0);
        var rawNames = reader.GetField<string>(1)!;
        var rawAbbreviations = reader.GetField<string>(2)!;

        double? grams = reader.TryGetField<double>(3, out var gramEquivalent) ? gramEquivalent : null;
        double? milliliters = reader.TryGetField<double>(4, out var milliliterEquivalent) ? milliliterEquivalent : null;

        var isUniversal = reader.TryGetField<bool>(5, out var universal) && universal;
        var isMeasurable = !(reader.TryGetField<bool>(6, out var unmeasurable) && unmeasurable);

        var names = ToQuantityNames(rawNames);
        var abbreviations = ToQuantityNames(rawAbbreviations);

        return (id, new QuantityUnit(names, abbreviations, grams, milliliters, isUniversal, isMeasurable));
    }

    private static QuantityNames ToQuantityNames(string rawNames)
    {
        var names = rawNames.Split(';');
        return names.Length == 1 ? new QuantityNames(names[0]) : new QuantityNames(names[0], names[1], names[2]);
    }

    private List<(int Id, QuantityUnit Unit)> ReadUnits()
    {
        if (!File.Exists(_options.FilePath))
        {
            _logger.LogError("File {OptionsFilePath} not found", _options.FilePath);
            return new List<(int, QuantityUnit)>();
        }

        var units = new List<(int, QuantityUnit)>();

        using var streamReader = new StreamReader(_options.FilePath);
        using var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        reader.Read();

        while (reader.Read())
        {
            var (id, unit) = ReadUnit(reader);
            units.Add((id, unit));
        }

        return units;
    }

    public IReadOnlyList<QuantityUnit> GetAllUnits()
    {
        if (HasCache)
        {
            _logger.LogDebug("Returning cached units");
            return _cachedUnits!.Values.ToArray();
        }

        var units = ReadUnitsAndTryToCache();
        return units.Values.ToArray();
    }

    private IReadOnlyDictionary<int, QuantityUnit> ReadUnitsAndTryToCache()
    {
        _logger.LogDebug("Reading units from file {OptionsFilePath}", _options.FilePath);
        var units = ReadUnits();
        var dictUnits = units.ToDictionary(u => u.Id, u => u.Unit);

        if (_options.CacheUnits)
        {
            _logger.LogDebug("Caching units");
            _cachedUnits = dictUnits;
        }

        return dictUnits;
    }

    public QuantityUnit? GetUnitById(int id)
    {
        if (id < 0)
        {
            _logger.LogError("Id {Id} is negative", id);
            return null;
        }

        if (HasCache)
        {
            return _cachedUnits!.TryGetValue(id, out var cachedUnit) ? cachedUnit : null;
        }

        var units = ReadUnitsAndTryToCache();
        return units.TryGetValue(id, out var unit) ? unit : null;
    }

    public int GetUnitId(QuantityUnit unit)
    {
        var units = GetStoredUnits();
        foreach (var pair in units)
        {
            if (pair.Value.Equals(unit))
            {
                return pair.Key;
            }
        }

        _logger.LogError("Unit {@Unit} not found. Returning 0", unit);
        return 0;
    }

    private bool HasCache => _options.CacheUnits && _cachedUnits is not null;

    private IReadOnlyDictionary<int, QuantityUnit> GetStoredUnits()
    {
        return HasCache ? _cachedUnits! : ReadUnitsAndTryToCache();
    }
}