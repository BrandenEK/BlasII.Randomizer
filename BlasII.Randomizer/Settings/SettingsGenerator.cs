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
                ShuffleLongQuests = false,
                ShuffleShops = true,
            },
            Preset.Quick => new RandomizerSettings()
            {
                Seed = RandomizerSettings.RANDOM_SEED,
                LogicType = 1,
                RequiredKeys = 1,
                StartingWeapon = 0,
                ShuffleLongQuests = false,
                ShuffleShops = false,
            },
            _ => throw new Exception($"Invalid preset type {preset}")
        };
    }

    ///// <summary>
    ///// The number of available presets
    ///// </summary>
    //public static int NumberOfPresets { get; } = Enum.GetNames(typeof(Preset)).Length;
}
