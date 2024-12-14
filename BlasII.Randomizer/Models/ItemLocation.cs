
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models location that contains a randomized item
/// </summary>
public class ItemLocation
{
    /// <summary>
    /// This location's identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// This location's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Flags that determine how this location will be shuffled
    /// </summary>
    public string Flags { get; set; }

    /// <summary>
    /// Temporary logic string
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// If this is a certain type of location, make sure the settings allow it to be shuffled
    /// </summary>
    public bool ShouldBeShuffled(RandomizerSettings settings)
    {
        if (Flags.Contains('L') && !settings.shuffleLongQuests)
            return false;

        if (Flags.Contains('S') && !settings.shuffleShops)
            return false;

        return true;
    }
}
