using Basalt.LogicParser;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Models;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Implementations;

public class PoolsItemShuffler : IShuffler
{
    private readonly Dictionary<string, ItemLocation> _allItemLocations;
    private readonly Dictionary<string, Item> _allItems;

    public PoolsItemShuffler(Dictionary<string, ItemLocation> itemLocations, Dictionary<string, Item> items)
    {
        _allItemLocations = itemLocations;
        _allItems = items;
    }

    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
    {
        output.Clear();
        var rng = new Random(seed);

        // Create pools of all locations to randomize
        LocationPool progressionLocations = new(rng), junkLocations = new(rng);
        CreateLocationPool(progressionLocations, junkLocations, settings);

        // Create pools of all items to randomize
        ItemPool progressionItems = new(rng), junkItems = new(rng);
        CreateItemPool(progressionItems, junkItems, progressionLocations.Size + junkLocations.Size, settings);

        // Create initial inventory
        var inventory = BlasphemousInventory.CreateNewInventory(settings);
        CreateInitialInventory(inventory, settings, progressionItems);

        // Place progression items at progression locations
        FillProgressionItems(progressionLocations, progressionItems, output, inventory);

        // Add junk items to remaining progression items and ensure they are still balanced
        junkLocations.Combine(progressionLocations);
        if (junkLocations.Size != junkItems.Size)
            return false;

        // Place junk items at junk locations and remaining progression locations
        FillJunkItems(junkLocations, junkItems, output);

        // Verify that all remaining items were placed
        return junkItems.Size == 0 && junkLocations.Size == 0;
    }

    #region Setup

    /// <summary>
    /// Fills the two location pools
    /// </summary>
    private void CreateLocationPool(LocationPool progressionLocations, LocationPool junkLocations, RandomizerSettings settings)
    {
        foreach (var location in _allItemLocations.Values)
        {
            AddLocationToPool(progressionLocations, junkLocations, location, settings);
        }
    }

    /// <summary>
    /// Takes a single location data and adds it to the correct list based on its type
    /// </summary>
    private void AddLocationToPool(LocationPool progressionLocations, LocationPool junkLocations, ItemLocation location, RandomizerSettings settings)
    {
        LocationPool locationPool = location.ShouldBeShuffled(settings) ? progressionLocations : junkLocations;
        locationPool.Add(location);
    }

    /// <summary>
    /// Fills the two item pools to match the number of locations
    /// </summary>
    private void CreateItemPool(ItemPool progressionItems, ItemPool junkItems, int numOfLocations, RandomizerSettings settings)
    {
        if (settings.AddPenitenceRewards)
            AddPenitenceItemsToPool(junkItems);

        foreach (var item in _allItems.Values)
        {
            AddItemToPool(progressionItems, junkItems, item);
        }

        RemoveStartingItemsFromItemPool(progressionItems, settings);
        BalanceItemPool(progressionItems, junkItems, numOfLocations); // Don't add any items just before this!
    }

    /// <summary>
    /// Takes a single item data and adds it to the correct list based on its type
    /// </summary>
    private void AddItemToPool(ItemPool progressionItems, ItemPool junkItems, Item item)
    {
        ItemPool itemPool = item.Class == Item.ItemClass.Progression ? progressionItems : junkItems;
        for (int i = 0; i < item.Count; i++)
        {
            itemPool.Add(item);
        }
    }

    /// <summary>
    /// Adds the six penitence reward items to the junk item pool
    /// </summary>
    private void AddPenitenceItemsToPool(ItemPool junkItems)
    {
        string[] penitenceItems = ["PR103", "PR108", "FG101", "FG105", "FG106", "FG111"];

        foreach (string id in penitenceItems)
            junkItems.Add(_allItems[id]);
    }

    /// <summary>
    /// Removes the starting weapon from the item pool
    /// </summary>
    private void RemoveStartingItemsFromItemPool(ItemPool items, RandomizerSettings settings)
    {
        // Remove the extra starting weapon
        items.Remove(_allItems[GetStartingWeaponId(settings)]);
    }

    /// <summary>
    /// After creating the item pool, add or remove junk items to make it equal to the number of locations
    /// </summary>
    private void BalanceItemPool(ItemPool progressionItems, ItemPool junkItems, int numOfLocations)
    {
        // Remove tear items until pools are equal
        while (progressionItems.Size + junkItems.Size > numOfLocations)
        {
            junkItems.RemoveLast();
        }

        // Add tear items until pools are equal
        while (progressionItems.Size + junkItems.Size < numOfLocations)
        {
            junkItems.Add(_allItems["Tears[800]"]);
        }
    }

    /// <summary>
    /// Adds the starting weapon and all progression items to the initial inventory
    /// </summary>
    private void CreateInitialInventory(GameInventory inventory, RandomizerSettings settings, ItemPool progressionItems)
    {
        // Add the starting weapon
        inventory.Add(GetStartingWeaponId(settings));

        // Add all progression items in the pool
        foreach (var item in progressionItems)
        {
            inventory.Add(item.Id);
        }
    }

    /// <summary>
    /// Calculates the item id of the chosen starting weapon
    /// </summary>
    private string GetStartingWeaponId(RandomizerSettings settings)
    {
        return ((WEAPON_IDS)settings.RealStartingWeapon).ToString();
    }

    #endregion

    #region Shuffle

    /// <summary>
    /// Using a reverse fill algorithm, place the last item at a random reachable location
    /// </summary>
    private void FillProgressionItems(LocationPool locations, ItemPool items, Dictionary<string, string> output, GameInventory inventory)
    {
        // Verify that all locations are reachable
        foreach (var location in locations)
        {
            if (!inventory.Evaluate(location.Logic))
                return;
        }

        items.Shuffle();
        MovePriorityItems(items);
        var reachableLocations = new LocationPool(locations);

        while (reachableLocations.Size > 0 && items.Size > 0)
        {
            Item item = items.RemoveLast();
            inventory.Remove(item.Id);

            RemoveUnreachableLocations(reachableLocations, inventory);
            if (reachableLocations.Size == 0)
                return;

            ItemLocation location = reachableLocations.RemoveRandom();
            locations.Remove(location);
            output.Add(location.Id, item.Id);
        }
    }

    /// <summary>
    /// Without logic, place the last item at a random location
    /// </summary>
    private void FillJunkItems(LocationPool locations, ItemPool items, Dictionary<string, string> output)
    {
        items.Shuffle();

        while (locations.Size > 0 && items.Size > 0)
        {
            ItemLocation location = locations.RemoveLast();
            Item item = items.RemoveLast();

            output.Add(location.Id, item.Id);
        }
    }

    /// <summary>
    /// After shuffling the list of progression items, move certain items to the end to prevent failing seeds
    /// </summary>        
    private void MovePriorityItems(ItemPool progressionItems)
    {
        Item wallClimb = _allItems["WallClimb"];
        progressionItems.MoveToBeginning(wallClimb);
    }

    /// <summary>
    /// Finds all locations that are no longer reachable and removes them
    /// </summary>
    private void RemoveUnreachableLocations(LocationPool locations, GameInventory inventory)
    {
        locations.RemoveConditional(loc => !inventory.Evaluate(loc.Logic));
    }

    #endregion
}
