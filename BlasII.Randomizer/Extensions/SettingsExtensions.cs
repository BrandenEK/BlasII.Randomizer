using System.Text;

namespace BlasII.Randomizer.Extensions;

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

        var sb = new StringBuilder();
        sb.AppendLine("RANDOMIZER SETTINGS");
        sb.AppendLine($"{SEED_NAME}: {settings.Seed}");
        sb.AppendLine();
        sb.AppendLine($"{LOGIC_NAME}: {logic}");
        sb.AppendLine($"{KEYS_NAME}: {keys}");
        sb.AppendLine($"{WEAPON_NAME}: {weapon}");
        sb.AppendLine($"{QUESTS_NAME}: {settings.ShuffleLongQuests}");
        sb.AppendLine($"{SHOPS_NAME}: {settings.ShuffleShops}");

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

        var sb = new StringBuilder();
        sb.AppendLine($"{SEED_NAME}: {settings.Seed}");
        sb.AppendLine();
        sb.AppendLine(LINE);
        sb.AppendLine($" {LOGIC_NAME}: {logic}");
        sb.AppendLine($" {KEYS_NAME}: {keys}");
        sb.AppendLine($" {WEAPON_NAME}: {weapon}");
        sb.AppendLine($" {QUESTS_NAME}: {settings.ShuffleLongQuests}");
        sb.AppendLine($" {SHOPS_NAME}: {settings.ShuffleShops}");
        sb.AppendLine(LINE);

        return sb.ToString();
    }

    private static readonly string LINE = new('=', 35);
    private static readonly string SEED_NAME = "Seed";
    private static readonly string LOGIC_NAME = "Logic difficulty";
    private static readonly string KEYS_NAME = "Required keys";
    private static readonly string WEAPON_NAME = "Starting weapon";
    private static readonly string QUESTS_NAME = "Shuffle long quests";
    private static readonly string SHOPS_NAME = "Shuffle shops";
    private static readonly string[] LOGIC_OPTIONS = ["Easy", "Normal", "Hard"];
    private static readonly string[] WEAPON_OPTIONS = ["Veredicto", "Ruego", "Sarmiento", "Mea Culpa"];
}
