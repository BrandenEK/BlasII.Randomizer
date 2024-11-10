using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Inventory.PlayMaker;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// In the cherub tower, prevent any altars from actually fading
/// </summary>
[HarmonyPatch(typeof(FadeSprite), nameof(FadeSprite.DoFadeWithThisIndex))]
class Fade_Hide_Patch
{
    public static bool Prefix(FadeSprite __instance)
    {
        if (__instance.name != "Graphics" || CoreCache.Room.CurrentRoom?.Name != "Z2401")
            return true;

        ModLog.Info("Preventing fade of cherub altar");
        return false;
    }
}

///// <summary>
///// Prevents locking the starting weapon on intro cutscene
///// </summary>
//[HarmonyPatch(typeof(LockWeapon), nameof(LockWeapon.OnEnter))]
//class LockWeapon_OnEnter_Patch
//{
//    public static bool Prefix(LockWeapon __instance)
//    {
//        ModLog.Info("Prevent weapon lock");
//        __instance.Finish();
//        return false;
//    }
//}

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