using BlasII.ModdingAPI.Assets;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for Progressive Quest Items
/// </summary>
public static class ProgressiveItemExtensions
{
    private static readonly ProgressiveItemData _lullabies = new ProgressiveItemData("UL",
        new ProgressiveItemLevel("QI23", new AddItemFunction("QI23")),
        new ProgressiveItemLevel("QI24", new AddItemFunction("QI24"), new RemoveItemFunction("QI23")),
        new ProgressiveItemLevel("QI25", new AddItemFunction("QI25"), new RemoveItemFunction("QI24")),
        new ProgressiveItemLevel("QI26", new AddItemFunction("QI26"), new RemoveItemFunction("QI25")),
        new ProgressiveItemLevel("QI27", new AddItemFunction("QI27"), new RemoveItemFunction("QI26")));

    private static readonly ProgressiveItemData _sculptorTools = new ProgressiveItemData("ST",
        new ProgressiveItemLevel("QI12", new AddItemFunction("QI12")),
        new ProgressiveItemLevel("QI02", new AddItemFunction("QI02")),
        new ProgressiveItemLevel("QI11", new AddItemFunction("QI11")),
        new ProgressiveItemLevel("QI03", new AddItemFunction("QI03")),
        new ProgressiveItemLevel("QI01", new AddItemFunction("QI01")));

    private static readonly ProgressiveItemData _lacrimatorios = new ProgressiveItemData("IL",
        new ProgressiveItemLevel("QI106", new AddItemFunction("QI106")),
        new ProgressiveItemLevel("QI107", new AddItemFunction("QI107"), new RemoveItemFunction("QI106")),
        new ProgressiveItemLevel("QI108", new AddItemFunction("QI108"), new RemoveItemFunction("QI107")),
        new ProgressiveItemLevel("QI109", new ChangeItemFunction("QI110"), new RemoveItemFunction("QI108")),
        new ProgressiveItemLevel("QI111", new AddItemFunction("QI111"), new RemoveItemsFunction("QI109", "QI110"), new GrantAbilityFunction(ABILITY_IDS.GoldFlask)));

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
