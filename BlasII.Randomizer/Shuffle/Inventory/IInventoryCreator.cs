using Basalt.LogicParser;

namespace BlasII.Randomizer.Shuffle.Inventory;

internal interface IInventoryCreator
{
    public void Create(RandomizerSettings settings, ItemPool progItems, out GameInventory inventory);
}
