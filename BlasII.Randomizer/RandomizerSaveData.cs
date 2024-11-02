using BlasII.ModdingAPI.Persistence;
using System.Collections.Generic;

namespace BlasII.Randomizer;

internal class RandomizerSaveData : SaveData
{
    public Dictionary<string, string> mappedItems;

    public List<string> collectedLocations;
    public List<string> collectedItems;

    public RandomizerSettings settings;
}
