
namespace BlasII.Randomizer.Models;

/// <summary>
/// Represents how item/quest flags should be bypassed for a location
/// </summary>
public class QuestBypassInfo(string scene, string item, string location)
{
    /// <summary>
    /// The scene name that this bypass takes effect in
    /// </summary>
    public string Scene { get; set; } = scene;

    /// <summary>
    /// The item that is being checked for
    /// </summary>
    public string Item { get; set; } = item;

    /// <summary>
    /// The location id that should be used instead
    /// </summary>
    public string Location { get; set; } = location;
}
