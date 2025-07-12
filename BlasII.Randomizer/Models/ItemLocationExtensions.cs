
namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for the ItemLocation model
/// </summary>
public static class ItemLocationExtensions
{
    /// <summary>
    /// If this is a certain type of location, make sure the settings allow it to be shuffled
    /// </summary>
    public static bool ShouldBeShuffled(this ItemLocation location, RandomizerSettings settings)
    {
        if (string.IsNullOrEmpty(location.Flags))
            return true;

        //if (location.Flags.Contains('L') && !settings.ShuffleLongQuests)
        //    return false;

        //if (location.Flags.Contains('S') && !settings.ShuffleShops)
        //    return false;

        return true;
    }

    /// <summary>
    /// Checks if the location is marked with the specified flag
    /// </summary>
    public static bool HasFlag(this ItemLocation location, char flag)
    {
        return !string.IsNullOrEmpty(location.Flags) && location.Flags.Contains(flag);
    }
}
