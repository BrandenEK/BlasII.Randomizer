using System.Text;

namespace BlasII.Randomizer.Extensions;

public static class SettingsExtensions
{
    /// <summary>
    /// Formats the settings for the info popup
    /// </summary>
    public static string FormatInfo(this RandomizerSettings settings)
    {
        string[] logics = ["Easy", "Normal", "Hard"];
        string[] weapons = ["Veredicto", "Ruego", "Sarmiento", "Mea Culpa"];

        string logic = logics[settings.LogicType];
        string keys = settings.RequiredKeys == -1 ? "Random" : settings.RequiredKeys.ToString();
        string weapon = settings.StartingWeapon == -1 ? "Random" : weapons[settings.StartingWeapon];

        var sb = new StringBuilder();
        sb.AppendLine("RANDOMIZER SETTINGS");
        sb.AppendLine("Seed: " + settings.Seed);
        sb.AppendLine();
        sb.AppendLine("Logic difficulty: " + logic);
        sb.AppendLine("Required keys: " + keys);
        sb.AppendLine("Starting weapon: " + weapon);
        sb.AppendLine("Shuffle long quests: " + settings.ShuffleLongQuests);
        sb.AppendLine("Shuffle shops: " + settings.ShuffleShops);

        return sb.ToString();
    }

    /// <summary>
    /// Formats the settings for the spoiler
    /// </summary>
    public static string FormatSpoiler(this RandomizerSettings settings)
    {
        string[] logics = ["Easy", "Normal", "Hard"];
        string[] weapons = ["Veredicto", "Ruego", "Sarmiento", "Mea Culpa"];
        var line = new string('=', 35);

        string logic = logics[settings.LogicType];
        string keys = settings.RequiredKeys == -1 ? $"[{settings.RealRequiredKeys}]" : settings.RequiredKeys.ToString();
        string weapon = settings.StartingWeapon == -1 ? $"[{weapons[settings.RealStartingWeapon]}]" : weapons[settings.StartingWeapon];

        var sb = new StringBuilder();
        sb.AppendLine("Seed: " + settings.Seed);
        sb.AppendLine();
        sb.AppendLine(line);
        sb.AppendLine(" Logic difficulty: " + logic);
        sb.AppendLine(" Required keys: " + keys);
        sb.AppendLine(" Starting weapon: " + weapon);
        sb.AppendLine(" Shuffle long quests: " + settings.ShuffleLongQuests);
        sb.AppendLine(" Shuffle shops: " + settings.ShuffleShops);
        sb.AppendLine(line);

        return sb.ToString();
    }
}
