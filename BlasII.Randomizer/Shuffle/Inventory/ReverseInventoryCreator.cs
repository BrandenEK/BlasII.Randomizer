using Basalt.LogicParser;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Shuffle.Models;

namespace BlasII.Randomizer.Shuffle.Inventory;

internal class ReverseInventoryCreator : IInventoryCreator
{
    public void Create(RandomizerSettings settings, ItemPool progItems, out GameInventory inventory)
    {
        inventory = BlasphemousInventory.CreateNewInventory(settings);

        // Add the starting weapon
        string weaponId = ((WEAPON_IDS)settings.RealStartingWeapon).ToString();
        inventory.Add(weaponId);

        // Add all progression items in the pool
        foreach (var item in progItems)
        {
            inventory.Add(item.Id);
        }
    }
}
