using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;

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
