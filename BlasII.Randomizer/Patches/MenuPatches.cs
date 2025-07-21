using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Extensions;
using HarmonyLib;
using Il2CppSystem.Threading.Tasks;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Prevent ascending any saves
/// </summary>
[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.PopulateSlotInfo))]
class MainMenuWindowLogic_PopulateSlotInfo_Patch
{
    public static void Postfix(MainMenuWindowLogic __instance, SlotInfo info)
    {
        info.canConvertNGPlus = false;

        //ModLog.Warn(__instance.slotsList.transform.DisplayHierarchy(10, true));
    }
}


[HarmonyPatch(typeof(UISelectableSlotDoves), nameof(UISelectableSlotDoves.SetDoves))]
class t2
{
    public static void Postfix(UISelectableSlotDoves __instance, int doves)
    {
        ModLog.Error("Set doves: " + doves);

        //bool hasKey = true; //AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI63"]);
        //__instance.images[0].sprite = hasKey ? AssetStorage.QuestItems["QI63"].image : __instance.empty;
    }
}
[HarmonyPatch(typeof(UISelectableSlotDoves), nameof(UISelectableSlotDoves.UpdateSelected))]
class t4
{
    public static void Postfix(UISelectableSlotDoves __instance)
    {
        ModLog.Error("Update selected");

        for (int i = 0; i < 5; i++)
        {
            bool hasKey = (__instance.numDoves & (1 << i)) != 0;
            __instance.images[i].sprite = hasKey ? AssetStorage.QuestItems[KEY_IDS[i]].image : __instance.empty;
        }
    }

    private static readonly string[] KEY_IDS =
    [
        "QI63", "QI64", "QI65", "QI66", "QI67"
    ];
}


[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.GetSlotInfo))]
class t3
{
    public static void Postfix(MainMenuWindowLogic __instance, int index, Task<SlotInfo> __result)
    {
        ModLog.Error($"Getting slot info for slot {index}");

        ModLog.Info(Main.Randomizer.ItemHandler.MappedItems.Count);
        ModLog.Info(__result.Result.doves);
        //ModLog.Warn(__instance.slotsInfo.Count);

        //if (__instance.slotsInfo.Count > 0)
        //__instance.slotsInfo[__instance.slotsInfo.Count - 1].doves = index + 2;

        __result.Result.doves = index switch
        {
            0 => 0b01001,
            1 => 0b11111,
            2 => 0b10010,
            _ => 0b00000
        };
    }
}

[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.WaitForSlots))]
class t5
{
    public static void Postfix(MainMenuWindowLogic __instance)
    {
        ModLog.Error($"Waited for slots");

        //ModLog.Warn(__instance.slotsInfo.Count);

        //for (int i = 0; i < __instance.slotsInfo.Count; i++)
        //{
        //    if (i == 0)
        //        __instance.slotsInfo[i].doves = 0b00001;
        //    if (i == 1)
        //        __instance.slotsInfo[i].doves = 0b11111;
        //    if (i == 2)
        //        __instance.slotsInfo[i].doves = 0b10010;
        //}
    }
}
