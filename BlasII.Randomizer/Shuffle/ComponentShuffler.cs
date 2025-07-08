using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Fill;
using BlasII.Randomizer.Shuffle.Inventory;
using BlasII.Randomizer.Shuffle.Pools;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle;

public class ComponentShuffler : IShuffler
{
    private readonly Dictionary<string, ItemLocation> _allLocations;
    private readonly Dictionary<string, Item> _allItems;

    private readonly ILocationPoolCreator _locationPoolCreator;
    private readonly IItemPoolCreator _itemPoolCreator;
    private readonly IPoolBalancer _poolBalancer;
    private readonly IInventoryCreator _inventoryCreator;
    private readonly IFiller _progressionFiller;
    private readonly IFiller _junkFiller;

    public ComponentShuffler(Dictionary<string, ItemLocation> locations, Dictionary<string, Item> items, bool useReverseFill)
    {
        _allLocations = locations;
        _allItems = items;

        _locationPoolCreator = new LocationPoolCreator(locations.Values);
        _itemPoolCreator = new ItemPoolCreator(items);
        _poolBalancer = new PoolBalancer(items);
        _inventoryCreator = useReverseFill ? new ReverseInventoryCreator() : new ForwardInventoryCreator();
        _progressionFiller = useReverseFill ? new ReverseProgressionFiller(_allItems) : new ForwardProgressionFiller(_allItems);
        _junkFiller = new JunkFiller();
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
        _progressionFiller.Fill(progLocations, progItems, output, inventory);

        // Add junk items to remaining progression items and ensure they are still balanced
        junkLocations.Combine(progLocations);
        if (junkLocations.Size != junkItems.Size)
            return false;

        // Place junk items at junk locations and remaining progression locations
        _junkFiller.Fill(junkLocations, junkItems, output, inventory);

        // Verify that all remaining items were placed
        return junkItems.Size == 0 && junkLocations.Size == 0;
    }
}
