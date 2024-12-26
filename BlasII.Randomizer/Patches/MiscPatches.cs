using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Characters;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Inventory;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// In the cherub tower, prevent any altars from actually fading
/// </summary>
[HarmonyPatch(typeof(FadeSprite), nameof(FadeSprite.DoFadeWithThisIndex))]
class FadeSprite_DoFadeWithThisIndex_Patch
{
    public static bool Prefix(FadeSprite __instance)
    {
        if (__instance.name != "Graphics" || CoreCache.Room.CurrentRoom?.Name != "Z2401")
            return true;

        ModLog.Info("Preventing fade of cherub altar");
        return false;
    }
}

/// <summary>
/// Only load starting room after some time
/// </summary>
[HarmonyPatch(typeof(ShowQuote), nameof(ShowQuote.OnEnter))]
class ShowQuote_OnEnter_Patch
{
    public static void Postfix()
    {
        Main.Randomizer.LoadStartingRoom();
    }
}

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
/// Instantly complete inactive timer for dlc ghosts
/// </summary>
[HarmonyPatch(typeof(PlayerInactiveDetection), nameof(PlayerInactiveDetection.IsInactive))]
class PlayerInactiveDetection_IsInactive_Patch
{
    public static void Prefix(PlayerInactiveDetection __instance)
    {
        string scene = CoreCache.Room.CurrentRoom?.Name;
        if (scene != "Z2702" && scene != "Z2705" && scene != "Z2816" && scene != "Z2828")
            return;

        ModLog.Info("Skipping inactive timer");
        __instance.timeleft = 0;
    }
}
