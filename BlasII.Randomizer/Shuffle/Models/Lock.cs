using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Shuffle.Models;

/// <summary>
/// Represents an item that is locked to a location
/// </summary>
public class Lock(ItemLocation location, Item item, Lock.LockType type)
{
    /// <summary>
    /// The location info
    /// </summary>
    public ItemLocation Location { get; } = location;

    /// <summary>
    /// The item info
    /// </summary>
    public Item Item { get; } = item;

    /// <summary>
    /// The lock type
    /// </summary>
    public LockType Type { get; } = type;

    /// <summary>
    /// Determines how the lock will behave during shuffle
    /// </summary>
    [System.Flags]
    public enum LockType
    {
        /// <summary> Has no effect on inventory or output </summary>
        Nothing = 0x00,
        /// <summary> Adds the item to the inventory when reached </summary>
        AddToInventory = 0x01,
        /// <summary> Adds the location to the output when reached </summary>
        AddToOutput = 0x02,
        /// <summary> Adds to inventory and output when reached </summary>
        AddToBoth = AddToInventory | AddToOutput
    }
}
