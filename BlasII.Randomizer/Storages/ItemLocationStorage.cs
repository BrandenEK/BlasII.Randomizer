using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class ItemLocationStorage : BaseInfoStorage<ItemLocation>
{
    /// <inheritdoc/>
    public ItemLocationStorage() : base("item-locations.json", "item locations")
    {
        // Temporarily remove all cherub locations
        var validLocations = new List<ItemLocation>();

        foreach (var location in _values.Values)
        {
            if (!string.IsNullOrEmpty(location.Flags) && location.Flags.Contains('C'))
                continue;

            validLocations.Add(location);
        }

        ModLog.Info($"Removed {_values.Count - validLocations.Count} cherub locations!");

        _values.Clear();
        foreach (var location in validLocations)
        {
            _values.Add(location.Id, location);
        }
    }
}
