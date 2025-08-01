﻿using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Fill;
using BlasII.Randomizer.Shuffle.Inventory;
using BlasII.Randomizer.Shuffle.Locks;
using BlasII.Randomizer.Shuffle.Models;
using BlasII.Randomizer.Shuffle.Pools;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Implementations;

public class ComponentShuffler : IShuffler
{
    private readonly ILocationPoolCreator _locationPoolCreator;
    private readonly IItemPoolCreator _itemPoolCreator;
    private readonly IPoolBalancer _poolBalancer;
    private readonly ILockPlacer _lockPlacer;
    private readonly IInventoryCreator _inventoryCreator;
    private readonly IFiller _progressionFiller;
    private readonly IFiller _junkFiller;

    public ComponentShuffler(Dictionary<string, ItemLocation> locations, Dictionary<string, Item> items, bool useReverseFill)
    {
        Item fillerItem = items["TA|800"];
        Item wallclimbItem = items["WallClimb"];

        _locationPoolCreator = new LocationPoolCreator(locations.Values);
        _itemPoolCreator = new ItemPoolCreator(items);
        _poolBalancer = new PoolBalancer(fillerItem);
        _lockPlacer = useReverseFill ? new FakeLockPlacer() : new LockPlacer(locations, items);
        _inventoryCreator = useReverseFill ? new ReverseInventoryCreator() : new ForwardInventoryCreator();
        _progressionFiller = useReverseFill ? new ReverseProgressionFiller(wallclimbItem) : new ForwardProgressionFiller(wallclimbItem);
        _junkFiller = new JunkFiller();
    }

    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
    {
        try
        {
            ShuffleInternal(seed, settings, output);
            return true;
        }
        catch (ShuffleException ex)
        {
            //Console.WriteLine(ex.Message);
            return false;
        }
    }

    private void ShuffleInternal(int seed, RandomizerSettings settings, Dictionary<string, string> output)
    {
        output.Clear();
        var rng = new Random(seed);

        // Create pools of all locations to randomize
        _locationPoolCreator.Create(rng, settings, out LocationPool progLocations, out LocationPool junkLocations);

        // Create pools of all items to randomize
        _itemPoolCreator.Create(rng, settings, out ItemPool progItems, out ItemPool junkItems);

        // Add or remove junk items to the number of locations equal the number of items
        _poolBalancer.Balance(progLocations, junkLocations, progItems, junkItems);

        // Lock certain items & locations and remove them from the pool
        _lockPlacer.Place(settings, progLocations, progItems, out List<Lock> locks);

        // Verify that an equal number of locations and items were removed
        if (progLocations.Size + junkLocations.Size != progItems.Size + junkItems.Size)
            throw new ShuffleException("The pools were unbalanced after placing locks");

        // Create initial inventory
        _inventoryCreator.Create(settings, progItems, out GameInventory inventory);

        // Place progression items at progression locations
        _progressionFiller.FillProgression(progLocations, progItems, locks, output, inventory);

        // Verify that all locks have been placed
        if (locks.Count > 0)
            throw new ShuffleException("Not all locks were placed after progression fill");

        // Add junk items to remaining progression items
        junkLocations.Combine(progLocations);

        // Verify that the pools are still balanced
        if (junkLocations.Size != junkItems.Size)
            throw new ShuffleException("The pools were unbalanced after progression fill");

        // Place junk items at junk locations and remaining progression locations
        _junkFiller.FillJunk(junkLocations, junkItems, output);

        // Verify that all remaining items were placed
        if (junkItems.Size != 0 || junkLocations.Size != 0)
            throw new ShuffleException("There were remaining items/locations after junk fill");
    }
}
