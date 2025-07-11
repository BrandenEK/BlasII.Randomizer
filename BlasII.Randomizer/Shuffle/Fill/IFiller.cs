using Basalt.LogicParser;
using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Fill;

internal interface IFiller
{
    public void FillProgression(LocationPool locations, ItemPool items, List<Lock> locks, Dictionary<string, string> output, GameInventory inventory);

    public void FillJunk(LocationPool locations, ItemPool items, Dictionary<string, string> output);
}
