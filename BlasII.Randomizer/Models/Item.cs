
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models an item that will be given at each location
/// </summary>
public class Item
{
    /// <summary>
    /// This item's identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// This item's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Determines how this item will be handled
    /// </summary>
    public ItemType Type { get; set; }

    /// <summary>
    /// Whether this item can lead to logical progression
    /// </summary>
    public bool Progression { get; set; }

    /// <summary>
    /// How many copies of this item should be added to the item pool
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// A classification for items
    /// </summary>
    public enum ItemType
    {
        RosaryBead = 0,
        Prayer = 1,
        Figurine = 2,
        QuestItem = 3,
        Weapon = 4,
        Ability = 5,
        Cherub = 6,
        Tears = 20,
        Marks = 21,
        PreMarks = 22,
        Invalid = 99,
    }
}
