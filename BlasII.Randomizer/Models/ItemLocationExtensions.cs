
namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for the ItemLocation model
/// </summary>
public static class ItemLocationExtensions
{
    /// <summary>
    /// Checks if the location is marked with the specified flag
    /// </summary>
    public static bool HasFlag(this ItemLocation location, char flag)
    {
        return !string.IsNullOrEmpty(location.Flags) && location.Flags.Contains(flag);
    }

    /// <summary>
    /// Checks if the location should be displayed as a valid location
    /// </summary>
    public static bool ContributesPercentage(this ItemLocation location, RandomizerSettings settings)
    {
        if (!settings.ShuffleCherubs && location.HasFlag('C'))
            return false;

        return true;
    }
}
