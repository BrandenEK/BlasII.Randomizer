using System;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Represents how item/quest flags should be bypassed for a location
/// </summary>
public class QuestBypassInfo(string scene, string item, Func<bool> itemOverride)
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
    /// The method to override the item check with
    /// </summary>
    public Func<bool> ItemOverride { get; set; } = itemOverride;
}
