using Basalt.LogicParser;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Fill;

internal interface IFiller
{
    public void Fill(LocationPool locations, ItemPool items, Dictionary<string, string> output, GameInventory inventory);
}
