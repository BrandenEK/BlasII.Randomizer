using Basalt.LogicParser;
using BlasII.ModdingAPI.Assets;

namespace BlasII.Randomizer.Shuffle.Inventory;

internal class ForwardInventoryCreator : IInventoryCreator
{
    public void Create(RandomizerSettings settings, ItemPool progItems, out GameInventory inventory)
    {
        inventory = BlasphemousInventory.CreateNewInventory(settings);

        // Add the starting weapon
        string weaponId = ((WEAPON_IDS)settings.RealStartingWeapon).ToString();
        inventory.Add(weaponId);
    }
}
