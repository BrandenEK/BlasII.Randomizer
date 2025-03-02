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

    // Item shuffle (Pool)

    /// <summary>
    /// Whether locations that require a lot of time can contain progression
    /// </summary>
    public bool ShuffleLongQuests { get; set; }

    /// <summary>
    /// Whether locations that require a purchase can contain progression
    /// </summary>
    public bool ShuffleShops { get; set; }

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
    /// A unique ID based on the seed and all settings
    /// </summary>
    public ulong UniqueIdentifier
    {
        get
        {
            // Simple version to start with
            ulong id = (ulong)Seed;

            int logic = LogicType;
            if ((logic & 1) != 0) id |= 1 << 20; // Logic
            if ((logic & 2) != 0) id |= 1 << 21;
            int keys = RequiredKeys + 1;
            if ((keys & 1) != 0) id |= 1 << 22; // Keys
            if ((keys & 2) != 0) id |= 1 << 23;
            if ((keys & 4) != 0) id |= 1 << 24;
            int weapon = StartingWeapon + 1;
            if ((weapon & 1) != 0) id |= 1 << 25; // Weapon
            if ((weapon & 2) != 0) id |= 1 << 26;
            if ((weapon & 4) != 0) id |= 1 << 27;
            if (ShuffleLongQuests) id |= 1 << 28; // Quests
            if (ShuffleShops) id |= 1 << 29; // Shops

            return id;
        }
    }

    /// <summary>
    /// A new settings object with default properties
    /// </summary>
    public static RandomizerSettings DEFAULT => new()
    {
        Seed = RANDOM_SEED,
        LogicType = 1,
        RequiredKeys = 4,
        StartingWeapon = -1,
        ShuffleLongQuests = false,
        ShuffleShops = true,
    };

    /// <summary>
    /// A random seed in the valid range
    /// </summary>
    public static int RANDOM_SEED => new Random().Next(1, MAX_SEED + 1);

    /// <summary>
    /// The maximum seed allowed
    /// </summary>
    public const int MAX_SEED = 999_999;
}