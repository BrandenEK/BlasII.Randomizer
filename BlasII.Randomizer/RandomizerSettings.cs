using BlasII.Randomizer.Settings;
using Newtonsoft.Json;
using System;

namespace BlasII.Randomizer;

/// <summary>
/// Selected options for the current randomizer run
/// </summary>
public class RandomizerSettings
{
    /// <summary>
    /// The seed
    /// </summary>
    public int Seed { get; set; }

    // Item shuffle (Functionality)

    /// <summary>
    /// Determines whether more difficult tricks will be required in logic
    /// </summary>
    public int LogicType { get; set; }

    /// <summary>
    /// Determines how many keys are required to open the door to CR
    /// </summary>
    public int RequiredKeys { get; set; }

    /// <summary>
    /// Determines which weapon you will start with
    /// </summary>
    public int StartingWeapon { get; set; }

    // Gameplay

    /// <summary>
    /// Determines how expensive shop items will be
    /// </summary>
    public int ShopMultiplier { get; set; }

    // Item shuffle (Pool)

    /// <summary>
    /// Should the penitence items be added to the pool
    /// </summary>
    public bool AddPenitenceRewards { get; set; }

    /// <summary>
    /// Whether locations that require a lot of time can contain progression
    /// </summary>
    public bool ShuffleLongQuests { get; set; }

    /// <summary>
    /// The actual starting weapon with "Random" taken into account
    /// </summary>
    [JsonIgnore]
    public int RealStartingWeapon
    {
        get
        {
            if (StartingWeapon == -1)
                return new Random(Seed).Next(0, 4);

            return StartingWeapon;
        }
    }

    /// <summary>
    /// The actual required keys with "Random" taken into account
    /// </summary>
    [JsonIgnore]
    public int RealRequiredKeys
    {
        get
        {
            if (RequiredKeys == -1)
                return new Random(Seed).Next(0, 6);

            return RequiredKeys;
        }
    }

    /// <summary>
    /// A random seed in the valid range
    /// </summary>
    public static int RANDOM_SEED => new Random().Next(1, MAX_SEED + 1);
}