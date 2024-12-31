using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Characters;
using Il2CppTGK.Game.Managers;

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
/// Give start equipment if player just spawned in a new game
/// </summary>
[HarmonyPatch(typeof(AbilityLockManager), nameof(AbilityLockManager.OnPlayerSpawned))]
class AbilityLockManager_OnPlayerSpawned_Patch
{
    public static void Postfix()
    {
        if (Main.Randomizer.IsNewGame)
            Main.Randomizer.GiveStartingEquipment();
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
