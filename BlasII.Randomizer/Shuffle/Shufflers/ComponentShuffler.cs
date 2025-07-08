using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Inventory;
using BlasII.Randomizer.Shuffle.Pools;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Shufflers;

public class ComponentShuffler : IShuffler
{
    private readonly Dictionary<string, ItemLocation> _allLocations;
    private readonly Dictionary<string, Item> _allItems;

    private readonly ILocationPoolCreator _locationPoolCreator;
    private readonly IItemPoolCreator _itemPoolCreator;
    private readonly IPoolBalancer _poolBalancer;
    private readonly IInventoryCreator _inventoryCreator;

    public ComponentShuffler(Dictionary<string, ItemLocation> locations, Dictionary<string, Item> items, bool useReverseFill)
    {
        _allLocations = locations;
        _allItems = items;

        _locationPoolCreator = new LocationPoolCreator(locations.Values);
        _itemPoolCreator = new ItemPoolCreator(items);
        _poolBalancer = new PoolBalancer(items);
        _inventoryCreator = useReverseFill ? new ReverseInventoryCreator() : new ForwardInventoryCreator();
    }

    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
    {
        output.Clear();
        var rng = new Random(seed);

        // Create pools of all locations to randomize
        _locationPoolCreator.Create(rng, settings, out LocationPool progLocations, out LocationPool junkLocations);

        // Create pools of all items to randomize
        _itemPoolCreator.Create(rng, settings, out ItemPool progItems, out ItemPool junkItems);

        // Add or remove junk items to the number of locations equal the number of items
        _poolBalancer.Balance(progLocations, junkLocations, progItems, junkItems);

        // Create initial inventory
        _inventoryCreator.Create(settings, progItems, out GameInventory inventory);

        // Place progression items at progression locations
        FillProgressionItems(progLocations, progItems, output, inventory);

        // Add junk items to remaining progression items and ensure they are still balanced
        junkLocations.Combine(progLocations);
        if (junkLocations.Size != junkItems.Size)
            return false;

        // Place junk items at junk locations and remaining progression locations
        FillJunkItems(junkLocations, junkItems, output);

        // Verify that all remaining items were placed
        return junkItems.Size == 0 && junkLocations.Size == 0;
    }

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
}
