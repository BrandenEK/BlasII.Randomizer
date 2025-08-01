﻿using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Fill;

internal class ReverseProgressionFiller : IFiller
{
    private readonly Item _wallclimbItem;

    public ReverseProgressionFiller(Item wallclimbItem)
    {
        _wallclimbItem = wallclimbItem;
    }

    public void FillJunk(LocationPool locations, ItemPool items, Dictionary<string, string> output)
    {
        throw new System.NotImplementedException();
    }

    public void FillProgression(LocationPool locations, ItemPool items, List<Lock> locks, Dictionary<string, string> output, GameInventory inventory)
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

    private void MovePriorityItems(ItemPool progressionItems)
    {
        progressionItems.MoveToBeginning(_wallclimbItem);
    }

    private void RemoveUnreachableLocations(LocationPool locations, GameInventory inventory)
    {
        locations.RemoveConditional(loc => !inventory.Evaluate(loc.Logic));
    }
}
