using BlasII.Randomizer.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Implementations;

public class DebugShuffler : IShuffler
{
    private readonly Dictionary<string, ItemLocation> _allItemLocations;
    private readonly string _item;

    public DebugShuffler(Dictionary<string, ItemLocation> itemLocations, string item)
    {
        _allItemLocations = itemLocations;
        _item = item;
    }

    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
    {
        foreach (string locationId in _allItemLocations.Keys)
        {
            output.Add(locationId, _item);
        }

        return true;
    }
}
