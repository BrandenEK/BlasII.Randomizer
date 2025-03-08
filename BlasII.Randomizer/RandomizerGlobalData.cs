using BlasII.ModdingAPI.Persistence;

namespace BlasII.Randomizer;

/// <summary>
/// Global save data for the Randomizer
/// </summary>
public class RandomizerGlobalData : GlobalSaveData
{
    /// <summary>
    /// The total number of seeds generated since v2.2.1
    /// </summary>
    public int SeedsGenerated { get; set; }
}
