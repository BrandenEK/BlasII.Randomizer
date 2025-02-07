using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores the names of various objects
/// </summary>
public class NameStorage
{
    private readonly Dictionary<string, string> _zoneNames;

    /// <summary>
    /// Initializes the storage by loading the names from txt files
    /// </summary>
    public NameStorage()
    {
        _zoneNames = LoadNames("names_zones.txt", "zone");
    }

    private Dictionary<string, string> LoadNames(string fileName, string type)
    {
        if (!Main.Randomizer.FileHandler.LoadDataAsArray(fileName, out string[] lines))
        {
            ModLog.Error($"Failed to load {type} names!");
            return [];
        }

        var nameDict = lines
            .Select(x => x.Split(':'))
            .ToDictionary(x => x[0].Trim(), x => x[1].Trim());

        ModLog.Info($"Loaded {nameDict.Count} {type} names!");
        return nameDict;
    }

    /// <summary>
    /// Gets the zone name for the specified item location
    /// </summary>
    public string GetZoneName(ItemLocation location)
    {
        return _zoneNames.TryGetValue(location.Id[..3], out string name) ? name : "[Unknown]";
    }
}
