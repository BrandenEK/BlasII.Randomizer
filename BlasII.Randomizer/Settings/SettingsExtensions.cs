using System.Text;

namespace BlasII.Randomizer.Settings;

/// <summary>
/// String formatter methods for the RandomizerSettings
/// </summary>
public static class SettingsExtensions
{
    /// <summary>
    /// Formats the settings for the info popup
    /// </summary>
    public static string FormatInfo(this RandomizerSettings settings)
    {
        string logic = LOGIC_OPTIONS[settings.LogicType];
        string keys = settings.RequiredKeys == -1 ? "Random" : settings.RequiredKeys.ToString();
        string weapon = settings.StartingWeapon == -1 ? "Random" : WEAPON_OPTIONS[settings.StartingWeapon];
        string shops = SHOP_OPTIONS[settings.ShopMultiplier];
        string marks = MARK_OPTIONS[settings.MartyrdomExperience];

        var sb = new StringBuilder();
        sb.AppendLine("RANDOMIZER SETTINGS");
        sb.AppendLine($"{SEED_NAME}: {settings.Seed}");
        sb.AppendLine();
        sb.AppendLine($"{LOGIC_NAME}: {logic}");
        sb.AppendLine($"{KEYS_NAME}: {keys}");
        sb.AppendLine($"{WEAPON_NAME}: {weapon}");
        sb.AppendLine($"{SHOPS_NAME}: {shops}");
        sb.AppendLine($"{MARKS_NAME}: {marks}");
        sb.AppendLine($"{PENITENCE_NAME}: {settings.AddPenitenceRewards}");
        sb.AppendLine($"{CHERUB_NAME}: {settings.ShuffleCherubs}");
        //sb.AppendLine($"{QUESTS_NAME}: {settings.ShuffleLongQuests}");

        return sb.ToString();
    }

    /// <summary>
    /// Formats the settings for the spoiler
    /// </summary>
    public static string FormatSpoiler(this RandomizerSettings settings)
    {
        string logic = LOGIC_OPTIONS[settings.LogicType];
        string keys = settings.RequiredKeys == -1 ? $"[{settings.RealRequiredKeys}]" : settings.RequiredKeys.ToString();
        string weapon = settings.StartingWeapon == -1 ? $"[{WEAPON_OPTIONS[settings.RealStartingWeapon]}]" : WEAPON_OPTIONS[settings.StartingWeapon];
        string shops = SHOP_OPTIONS[settings.ShopMultiplier];
        string marks = MARK_OPTIONS[settings.MartyrdomExperience];

        var sb = new StringBuilder();
        sb.AppendLine($"{SEED_NAME}: {settings.Seed}");
        sb.AppendLine();
        sb.AppendLine(LINE);
        sb.AppendLine($" {LOGIC_NAME}: {logic}");
        sb.AppendLine($" {KEYS_NAME}: {keys}");
        sb.AppendLine($" {WEAPON_NAME}: {weapon}");
        sb.AppendLine($" {SHOPS_NAME}: {shops}");
        sb.AppendLine($" {MARKS_NAME}: {marks}");
        sb.AppendLine($" {PENITENCE_NAME}: {settings.AddPenitenceRewards}");
        sb.AppendLine($" {CHERUB_NAME}: {settings.ShuffleCherubs}");
        //sb.AppendLine($" {QUESTS_NAME}: {settings.ShuffleLongQuests}");
        sb.AppendLine(LINE);

        return sb.ToString();
    }

    /// <summary>
    /// Calculates a unique ID based on the seed and all settings
    /// </summary>
    public static ulong CalculateUID(this RandomizerSettings settings)
    {
        ulong uid = 0;
        int idx = 0;
        bool flip = false;

        // Seed
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_02) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_04) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_08) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_10) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_20) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_40) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_00_80) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_01_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_02_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_04_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_08_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_10_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_20_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_40_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_00_80_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_01_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_02_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_04_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_08_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_10_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_20_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_40_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x00_80_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x01_00_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x02_00_00_00) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.Seed & 0x04_00_00_00) > 0);

        // LogicType
        SetBit(ref uid, ref idx, ref flip, (settings.LogicType + 1 & 0x01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.LogicType + 1 & 0x02) > 0);

        // RequiredKeys
        SetBit(ref uid, ref idx, ref flip, (settings.RequiredKeys + 1 & 0x01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.RequiredKeys + 1 & 0x02) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.RequiredKeys + 1 & 0x04) > 0);

        // StartingWeapon
        SetBit(ref uid, ref idx, ref flip, (settings.StartingWeapon + 1 & 0x01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.StartingWeapon + 1 & 0x02) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.StartingWeapon + 1 & 0x04) > 0);

        // ShopMultiplier
        SetBit(ref uid, ref idx, ref flip, (settings.ShopMultiplier & 0x01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.ShopMultiplier & 0x02) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.ShopMultiplier & 0x04) > 0);

        // MartyrdomExperience
        SetBit(ref uid, ref idx, ref flip, (settings.MartyrdomExperience & 0x01) > 0);
        SetBit(ref uid, ref idx, ref flip, (settings.MartyrdomExperience & 0x02) > 0);

        // Shuffle
        SetBit(ref uid, ref idx, ref flip, settings.AddPenitenceRewards);
        SetBit(ref uid, ref idx, ref flip, settings.ShuffleCherubs);
        //SetBit(ref uid, ref idx, ref flip, settings.ShuffleLongQuests);

        return uid;
    }

    /// <summary>
    /// Sets a bit for the UID based on the bit order and flip status
    /// </summary>
    private static void SetBit(ref ulong uid, ref int idx, ref bool flip, bool value)
    {
        byte bit = BIT_ORDER[idx++];
        flip = !flip;

        if (value ^ flip)
            uid |= (ulong)1 << bit;
    }

    private static readonly string LINE = new('=', 35);
    private static readonly string SEED_NAME = "Seed";
    private static readonly string LOGIC_NAME = "Logic difficulty";
    private static readonly string KEYS_NAME = "Required keys";
    private static readonly string WEAPON_NAME = "Starting weapon";
    private static readonly string SHOPS_NAME = "Shop costs";
    private static readonly string MARKS_NAME = "Martyrdom XP";
    private static readonly string PENITENCE_NAME = "Add penitence rewards";
    private static readonly string CHERUB_NAME = "Shuffle cherubs";
    private static readonly string QUESTS_NAME = "Shuffle long quests";
    private static readonly string[] LOGIC_OPTIONS = ["Easy", "Normal", "Hard"];
    private static readonly string[] WEAPON_OPTIONS = ["Veredicto", "Ruego", "Sarmiento", "Mea Culpa"];
    private static readonly string[] SHOP_OPTIONS = ["Free", "Cheap", "Standard", "Expensive", "Vanilla"];
    private static readonly string[] MARK_OPTIONS = ["Vanilla", "Double XP", "From items", "From bosses"];

    private static readonly byte[] BIT_ORDER =
    [
        35, 30, 46, 34, 33, 12, 16, 25, 01, 22, 05, 07, 21, 36, 38,
        09, 20, 42, 24, 29, 59, 11, 54, 41, 40, 53, 15, 39, 52, 32,
        57, 02, 23, 45, 55, 43, 19, 27, 04, 44, 28, 58, 17, 13, 18,
        06, 47, 37, 14, 10, 26, 03, 56, 51, 08, 49, 48, 31, 00, 50,
    ];
}
