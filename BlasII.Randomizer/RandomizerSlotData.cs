using BlasII.ModdingAPI.Persistence;
using System.Collections.Generic;

namespace BlasII.Randomizer;

/// <summary>
/// Slot save data for the Randomizer
/// </summary>
public class RandomizerSlotData : SlotSaveData
{
    /// <summary>
    /// The location to item mapping
    /// </summary>
    public Dictionary<string, string> mappedItems;

    /// <summary>
    /// The list of collected locations
    /// </summary>
    public List<string> collectedLocations;

    /// <summary>
    /// The list of collected items
    /// </summary>
    public List<string> collectedItems;

    /// <summary>
    /// The selected settings
    /// </summary>
    public RandomizerSettings settings;
}
