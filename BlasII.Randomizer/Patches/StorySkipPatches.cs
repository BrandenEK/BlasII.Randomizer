using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Skip showing the map popup at the beginning - Probably unnecessary since the trigger is removed
/// </summary>
[HarmonyPatch(typeof(ShowMapDestinationTutorial), nameof(ShowMapDestinationTutorial.OnEnter))]
class ShowMapDestinationTutorial_OnEnter_Patch
{
    public static bool Prefix(ShowMapDestinationTutorial __instance)
    {
        ModLog.Warn("Skipping map event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}

/// <summary>
/// Skip showing sorrows popups after bosses
/// </summary>
[HarmonyPatch(typeof(ShowSorrowsPopup), nameof(ShowSorrowsPopup.OnEnter))]
class ShowSorrowsPopup_OnEnter_Patch
{
    public static bool Prefix(ShowSorrowsPopup __instance)
    {
        ModLog.Warn("Skipping sorrows event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}

/// <summary>
/// Skip showing dove popups after bosses
/// </summary>
[HarmonyPatch(typeof(ShowDovePopup), nameof(ShowDovePopup.OnEnter))]
class ShowDovePopup_OnEnter_Patch
{
    public static bool Prefix(ShowDovePopup __instance)
    {
        ModLog.Warn("Skipping dove event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}
