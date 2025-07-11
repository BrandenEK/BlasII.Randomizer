using Basalt.LogicParser;
using BlasII.Randomizer.Shuffle.Models;

namespace BlasII.Randomizer.Shuffle.Inventory;

internal interface IInventoryCreator
{
    public void Create(RandomizerSettings settings, ItemPool progItems, out GameInventory inventory);
}
