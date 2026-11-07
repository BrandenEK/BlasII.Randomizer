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
/// Adds an item to the inventory
/// </summary>
public class AddItemFunction(string item) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
        AssetStorage.PlayerInventory.AddItemAsync(AssetStorage.QuestItems[item]);
    }
}

/// <summary>
/// Removes a single item from the inventory
/// </summary>
public class RemoveItemFunction(string item) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
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
            AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[item]);
        }
    }
}

/// <summary>
/// Changes the item into a different one
/// </summary>
public class ChangeItemFunction(string item) : IFunction
{
    /// <inheritdoc/>
    public void Execute()
    {
        AssetStorage.PlayerInventory.AddItemAsync(AssetStorage.QuestItems[item]);
        Main.Randomizer.ItemHandler.DisplayItem(Main.Randomizer.ItemStorage[item]);
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
        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ability], true);
    }
}
