using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Storages;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for Progressive Quest Items
/// </summary>
public static class ProgressiveItemExtensions
{
    private static readonly ProgressiveItemData _lullabies = new ProgressiveItemData("UL",
        new ProgressiveItemLevel("QI23"),
        new ProgressiveItemLevel("QI24", new RemoveItem("QI23")),
        new ProgressiveItemLevel("QI25", new RemoveItem("QI24")),
        new ProgressiveItemLevel("QI26", new RemoveItem("QI25")),
        new ProgressiveItemLevel("QI27", new RemoveItem("QI26")));

    private static readonly ProgressiveItemData _sculptorTools = new ProgressiveItemData("ST",
        new ProgressiveItemLevel("QI12"),
        new ProgressiveItemLevel("QI02"),
        new ProgressiveItemLevel("QI11"),
        new ProgressiveItemLevel("QI03"),
        new ProgressiveItemLevel("QI01"));

    private static readonly ProgressiveItemData _lacrimatorios = new ProgressiveItemData("IL",
        new ProgressiveItemLevel("QI106"),
        new ProgressiveItemLevel("QI107", new RemoveItem("QI106")),
        new ProgressiveItemLevel("QI108", new RemoveItem("QI107")),
        new ProgressiveItemLevel("QI110", new RemoveItem("QI108")),
        new ProgressiveItemLevel("QI111", new RemoveItems("QI109", "QI110"), new GrantAbility(ABILITY_IDS.GoldFlask)));

    /// <summary>
    /// Gets the progressive data for the specified item
    /// </summary>
    public static ProgressiveItemData GetProgressiveData(this Item item)
    {
        return item.Id switch
        {
            "IL" => _lacrimatorios,
            "ST" => _sculptorTools,
            "UL" => _lullabies,
            _ => throw new System.Exception($"Invalid progressive questitem id: {item.Id}")
        };
    }

    /// <summary>
    /// Gets the item model for the next level of the specified item
    /// </summary>
    public static Item GetNextProgressiveItem(this Item item)
    {
        var data = GetProgressiveData(item);
        var nextLevel = data.NextProgressiveLevel;

        return Main.Randomizer.ItemStorage[nextLevel.Id];
    }
}
