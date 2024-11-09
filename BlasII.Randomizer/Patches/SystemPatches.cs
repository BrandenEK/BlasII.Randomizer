using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Always allow upgrading weapons, even without lance
/// </summary>
[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.HasItem))]
class Inventory_HasItem_Patch
{
    public static void Postfix(ItemID itemID, ref bool __result)
    {
        __result = __result || itemID.name == "QI70" && Main.Randomizer.GetQuestBool("ST00", "WEAPON_EVENT");
    }
}

/// <summary>
/// When reloading a boss room after the fight, force deactivate it to prevent camera lock
/// </summary>
[HarmonyPatch(typeof(RoomManager), nameof(RoomManager.ChangeRoom))]
class Room_Change_Patch
{
    public static void Prefix(int roomHash, ref bool forceDeactivate)
    {
        foreach (int room in bossRooms)
        {
            if (roomHash == room)
            {
                ModLog.Info("Force deactivating boss room");
                forceDeactivate = true;
            }
        }
    }

    private static readonly int[] bossRooms =
    {
        129108797,
        -1436975306,
        129108570,
        // 1574233179, Causes missing fog vfx
        // Benedicta changes rooms
        -133013164,
        1433070649,
        1433070681,
    };
}
