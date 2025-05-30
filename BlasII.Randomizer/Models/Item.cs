﻿
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models an item that will be given at each location
/// </summary>
public class Item : IUnique
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
    /// How this item is used for logical progression
    /// </summary>
    public ItemClass Class { get; set; }

    /// <summary>
    /// How many copies of this item should be added to the item pool
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// A classification for items
    /// </summary>
    public enum ItemType
    {
        /// <summary> An inventory item with RB </summary>
        RosaryBead,
        /// <summary> An inventory item with PR </summary>
        Prayer,
        /// <summary> An inventory item with FG </summary>
        Figurine,
        /// <summary> An inventory item with QI </summary>
        QuestItem,
        /// <summary> A progressive inventory item with QI </summary>
        ProgressiveQuestItem,
        /// <summary> An inventory item for ST103 </summary>
        GoldLump,
        /// <summary> A weapon or its upgrade </summary>
        Weapon,
        /// <summary> A movement ability </summary>
        Ability,
        /// <summary> A cherub </summary>
        Cherub,
        /// <summary> A certain amount of Tears of Atonement </summary>
        Tears,
        /// <summary> A certain amount of Marks of Martyrdom </summary>
        Marks,
        /// <summary> A certain amount of Marks of the Preceptor </summary>
        PreMarks,
        /// <summary> An invalid or missing item </summary>
        Invalid,
    }

    /// <summary>
    /// A classification of an item's importance
    /// </summary>
    public enum ItemClass
    {
        /// <summary> No special properties </summary>
        Filler,
        /// <summary> Nice to have </summary>
        Useful,
        /// <summary> Used for logical advancement </summary>
        Progression,
    }
}
