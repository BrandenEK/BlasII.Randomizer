using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Prevent ascending any saves
/// </summary>
[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.PopulateSlotInfo))]
class MainMenuWindowLogic_PopulateSlotInfo_Patch
{
    public static void Postfix(SlotInfo info)
    {
        info.canConvertNGPlus = false;
    }
}
