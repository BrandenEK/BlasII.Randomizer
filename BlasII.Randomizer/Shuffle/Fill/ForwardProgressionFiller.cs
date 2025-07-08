using Basalt.LogicParser;
using BlasII.Randomizer.Models;
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

    public void Fill(LocationPool locations, ItemPool items, Dictionary<string, string> output, GameInventory inventory)
    {
        items.Shuffle();
        MovePriorityItems(items);
        var reachableLocations = FindReachableLocations(locations, inventory);

        while (reachableLocations.Size > 0 && items.Size > 0)
        {
            ItemLocation location = reachableLocations.RemoveRandom();
            locations.Remove(location);

            Item item = items.RemoveLast();
            inventory.Add(item.Id);

            output.Add(location.Id, item.Id);
            reachableLocations = FindReachableLocations(locations, inventory);
        }
    }

    private void MovePriorityItems(ItemPool progressionItems)
    {
        Item wallClimb = _items["WallClimb"];
        progressionItems.MoveToEnd(wallClimb);
    }

    private LocationPool FindReachableLocations(LocationPool locations, GameInventory inventory)
    {
        var temp = new LocationPool(locations);
        temp.Clear();

        foreach (var location in locations.Where(x => inventory.Evaluate(x.Logic)))
        {
            temp.Add(location);
        }

        // Not ideal, but thats how the pools are set up
        return temp;
    }
}
