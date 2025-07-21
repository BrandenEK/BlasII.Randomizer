using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppSystem;
using Il2CppTGK.Game.Components.Projectiles;
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

        //ModLog.Warn(PrieuDieuMenuLogic.HUB_SCENE);
        //ModLog.Warn(PrieuDieuMenuLogic.HUB_PRIE_DIEU);

        PrieuDieuMenuLogic.HUB_SCENE = 1291908248;
        PrieuDieuMenuLogic.HUB_PRIE_DIEU = 931257951;
    }
}

/// <summary>
/// Actually warp to the city
/// </summary>
[HarmonyPatch(typeof(PR17_Logic), nameof(PR17_Logic.OnPrayerResumeTeleport))]
class PR17_Logic_OnPrayerResumeTeleport_Patch
{
    public static void Prefix()
    {
        ModLog.Info("Warping to city");

        //ModLog.Warn(PrieuDieuMenuLogic.HUB_SCENE);
        //ModLog.Warn(PrieuDieuMenuLogic.HUB_PRIE_DIEU);

        PrieuDieuMenuLogic.HUB_SCENE = 1291908116;
        PrieuDieuMenuLogic.HUB_PRIE_DIEU = 1878221899;
    }
}
