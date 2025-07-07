using System;

namespace BlasII.Randomizer.Settings;

/// <summary>
/// Handles creating settings objects
/// </summary>
public static class SettingsGenerator
{
    /// <summary>
    /// Generates a new settings object with the preset options
    /// </summary>
    public static RandomizerSettings CreateFromPreset(Preset preset)
    {
        return preset switch
        {
            Preset.Standard => new RandomizerSettings()
            {
                Seed = GetRandomSeed(),
                LogicType = 1,
                RequiredKeys = 4,
                StartingWeapon = -1,
                ShopMultiplier = 2,
                AddPenitenceRewards = true,
                ShuffleLongQuests = false,
            },
            Preset.Quick => new RandomizerSettings()
            {
                Seed = GetRandomSeed(),
                LogicType = 1,
                RequiredKeys = 1,
                StartingWeapon = 0,
                ShopMultiplier = 1,
                AddPenitenceRewards = true,
                ShuffleLongQuests = false,
            },
            _ => throw new Exception($"Invalid preset type {preset}")
        };
    }

    /// <summary>
    /// Generates a random seed deterministically
    /// </summary>
    public static int GetRandomSeed(Random rng)
    {
        return rng.Next(1, MAX_SEED + 1);
    }

    /// <summary>
    /// Generates a random seed undeterministically
    /// </summary>
    public static int GetRandomSeed()
    {
        return GetRandomSeed(new Random());
    }

    /// <summary>
    /// The maximum seed allowed
    /// </summary>
    public const int MAX_SEED = 99_999_999;
}
