using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Shuffle.Fill;

internal class ForwardProgressionFiller : IFiller
{
    private readonly Dictionary<string, Item> _items;

    public ForwardProgressionFiller(Dictionary<string, Item> items)
    {
        _items = items;
    }

    public void FillJunk(LocationPool locations, ItemPool items, Dictionary<string, string> output)
    {
        throw new System.NotImplementedException();
    }

    public void FillProgression(LocationPool locations, ItemPool items, List<Lock> locks, Dictionary<string, string> output, GameInventory inventory)
    {
        items.Shuffle();
        MovePriorityItems(items);

        var reachableLocations = new LocationPool(locations);
        var unreachableLocations = new LocationPool(locations);

        reachableLocations.Clear();
        UpdateReachableLocations(reachableLocations, unreachableLocations, locks, output, inventory);

        while (reachableLocations.Size > 0 && items.Size > 0)
        {
            ItemLocation location = reachableLocations.RemoveRandom();
            locations.Remove(location);

            Item item = items.RemoveLast();
            inventory.Add(item.Id);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("new pass: " + item.Id);

            output.Add(location.Id, item.Id);
            UpdateReachableLocations(reachableLocations, unreachableLocations, locks, output, inventory);
        }

        if (items.Size == 0 && unreachableLocations.Size > 0)
            throw new ShuffleException($"There were {unreachableLocations.Size} unreachable locations after progression fill");
    }

    private void MovePriorityItems(ItemPool progressionItems)
    {
        Item wallClimb = _items["WallClimb"];
        progressionItems.MoveToEnd(wallClimb);
    }

    private void UpdateReachableLocations(LocationPool reachable, LocationPool unreachable, List<Lock> locks, Dictionary<string, string> output, GameInventory inventory)
    {
        bool refresh = true;

        while (refresh)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Checking reachability");

            refresh = false;

            if (CheckPoolLocations(reachable, unreachable, inventory))
                refresh = true;
            if (CheckLockedLocations(locks, output, inventory))
                refresh = true;
        }
    }

    private bool CheckPoolLocations(LocationPool reachable, LocationPool unreachable, GameInventory inventory)
    {
        var newLocations = unreachable.Where(x => inventory.Evaluate(x.Logic)).ToArray();

        foreach (var location in newLocations)
        {
            unreachable.Remove(location);
            reachable.Add(location);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("New reachable: " + location.Id);
        }

        return false;
    }

    private bool CheckLockedLocations(List<Lock> locks, Dictionary<string, string> output, GameInventory inventory)
    {
        bool refresh = false;

        for (int i = 0; i < locks.Count; i++)
        {
            Lock lck = locks[i];

            if (!inventory.Evaluate(lck.Location.Logic))
                continue;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Placing locked item: " + lck.Location.Id);

            output.Add(lck.Location.Id, lck.Item.Id);
            inventory.Add(lck.Item.Id);

            locks.RemoveAt(i--);
            refresh = true;
        }

        return refresh;
    }
}
