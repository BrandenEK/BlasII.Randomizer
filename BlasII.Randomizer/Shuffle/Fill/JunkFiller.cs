using Basalt.LogicParser;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Fill;

internal class JunkFiller : IFiller
{
    public void FillJunk(LocationPool locations, ItemPool items, Dictionary<string, string> output)
    {
        items.Shuffle();

        while (locations.Size > 0 && items.Size > 0)
        {
            ItemLocation location = locations.RemoveLast();
            Item item = items.RemoveLast();

            output.Add(location.Id, item.Id);
        }
    }

    public void FillProgression(LocationPool locations, ItemPool items, List<Lock> locks, Dictionary<string, string> output, GameInventory inventory)
    {
        throw new System.NotImplementedException();
    }
}
