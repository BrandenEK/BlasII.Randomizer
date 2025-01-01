using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.PrieDieu;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Change what they are checking from owning the upgrade to the flag status
/// </summary>
[HarmonyPatch(typeof(IsPrieDieuUpgraded), nameof(IsPrieDieuUpgraded.OnEnter))]
class IsPrieDieuUpgraded_OnEnter_Patch
{
    public static bool Prefix(IsPrieDieuUpgraded __instance)
    {
        ModLog.Info($"Skipping prieu dieu upgrade: {__instance.prieDieuUpgrade.Value.name}");
        __instance.Fsm.Event(__instance.yesEvent);
        __instance.Finish();
        return false;
    }
}
