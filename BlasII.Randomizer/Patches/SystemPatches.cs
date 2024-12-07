﻿using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
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
        if (!Main.Randomizer.Data.TryGetBossTeleportInfo(roomHash, out BossTeleportInfo info))
            return;

        ModLog.Info("Force deactivating boss room: " + info.ForceDeactivate);
        forceDeactivate = info.ForceDeactivate;
    }
}
