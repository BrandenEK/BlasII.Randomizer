using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppSystem;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;

namespace BlasII.Randomizer.Patches;


[HarmonyPatch(typeof(PrieuDieuMenuLogic), nameof(PrieuDieuMenuLogic.AddElement), typeof(string), typeof(string), typeof(Action))]
class t
{
    public static void Prefix(string itemName, ref string caption)
    {
        //ModLog.Error("PD add: " + itemName + " - " + caption);
        if (itemName == "TeleportToHUBUpgrade")
        {
            caption = "Travel to starting room";
        }
    }
}

[HarmonyPatch(typeof(PrieuDieuMenuLogic), nameof(PrieuDieuMenuLogic.ActionTeleportToHud))]
class t1
{
    public static void Prefix(PrieuDieuMenuLogic __instance)
    {
        ModLog.Info("Warping to starting room");

        //ModLog.Warn(PrieuDieuMenuLogic.HUB_SCENE);
        //ModLog.Warn(PrieuDieuMenuLogic.HUB_PRIE_DIEU);
        //ModLog.Warn(PrieuDieuMenuLogic.HUB_ZONE);

        PrieuDieuMenuLogic.HUB_SCENE = 1291908248;
        PrieuDieuMenuLogic.HUB_PRIE_DIEU = 931257951;

        //__instance.FadeAndClose();
        //CoreCache.PlayerSpawn.TeleportPlayerToFirstType("Z0101", PlayerSpawnManager.SpawnTypes.Respawn, CoreCache.PlayerSpawn.spawnFirstEntryType);
        //return true;
    }
}
