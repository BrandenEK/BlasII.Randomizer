using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppSystem;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Change teleport option to the starting room
/// </summary>
[HarmonyPatch(typeof(PrieuDieuMenuLogic), nameof(PrieuDieuMenuLogic.AddElement), typeof(string), typeof(string), typeof(Action))]
class PrieuDieuMenuLogic_AddElement_Patch
{
    public static void Prefix(string itemName, ref string caption)
    {
        if (itemName == "TeleportToHUBUpgrade")
            caption = Main.Randomizer.LocalizationHandler.Localize("dialog/travel");
    }
}

/// <summary>
/// Actually warp to the starting room
/// </summary>
[HarmonyPatch(typeof(PrieuDieuMenuLogic), nameof(PrieuDieuMenuLogic.ActionTeleportToHud))]
class PrieuDieuMenuLogic_ActionTeleportToHud_Patch
{
    public static void Prefix()
    {
        ModLog.Info("Warping to starting room");

        PrieuDieuMenuLogic.HUB_SCENE = 1291908248;
        PrieuDieuMenuLogic.HUB_PRIE_DIEU = 931257951;
    }
}
