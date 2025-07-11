using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Shuffle.Models;

/// <summary>
/// Represents an item that is locked to a location
/// </summary>
public class Lock(ItemLocation location, Item item)
{
    /// <summary>
    /// The location info
    /// </summary>
    public ItemLocation Location { get; } = location;

    /// <summary>
    /// The item info
    /// </summary>
    public Item Item { get; } = item;
}
