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
                Seed = RandomizerSettings.RANDOM_SEED,
                LogicType = 1,
                RequiredKeys = 4,
                StartingWeapon = -1,
                ShopMultiplier = 2,
                AddPenitenceRewards = true,
                ShuffleLongQuests = false,
            },
            Preset.Quick => new RandomizerSettings()
            {
                Seed = RandomizerSettings.RANDOM_SEED,
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
    /// The maximum seed allowed
    /// </summary>
    public const int MAX_SEED = 99_999_999;
}
