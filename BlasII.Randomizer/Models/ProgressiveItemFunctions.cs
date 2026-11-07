using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using System.Collections.Generic;

namespace BlasII.Randomizer.Models;

/// <summary>
/// A function that should be run when a progressive quest item is received
/// </summary>
public interface IFunction
{
    /// <summary>
    /// Executes the functionality
    /// </summary>
    public void Execute();
}

/// <summary>
/// Removes a single item from the inventory
/// </summary>
public class RemoveItemFunction(string item) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
        ModLog.Warn("Removing item " + item);
        AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[item]);
    }
}

/// <summary>
/// Removes multiple items from the inventory
/// </summary>
public class RemoveItemsFunction(params List<string> items) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
        foreach (string item in items)
        {
            ModLog.Warn("Removing item " + item);
            AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[item]);
        }
    }
}

/// <summary>
/// Grants a player ability
/// </summary>
public class GrantAbilityFunction(ABILITY_IDS ability) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
        ModLog.Warn($"Granting ability {ability}");
        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ability], true);
    }
}
