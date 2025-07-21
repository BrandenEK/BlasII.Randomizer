using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
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
    }
}

/// <summary>
/// Loads the collected keys and updates the slot UI
/// </summary>
[HarmonyPatch(typeof(UISelectableSlotDoves), nameof(UISelectableSlotDoves.UpdateSelected))]
class UISelectableSlotDoves_UpdateSelected_Patch
{
    public static void Postfix(UISelectableSlotDoves __instance)
    {
        for (int i = 0; i < 5; i++)
        {
            bool hasKey = (__instance.numDoves & (1 << i)) != 0;
            __instance.images[4 - i].sprite = hasKey ? AssetStorage.QuestItems[Items.KEY_IDS[i]].image : __instance.empty;
        }
    }
}

/// <summary>
/// Saves the collected keys to SlotInfo
/// </summary>
[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.GetSlotInfo))]
class MainMenuWindowLogic_GetSlotInfo_Patch
{
    public static void Postfix(MainMenuWindowLogic __instance, int index, Task<SlotInfo> __result)
    {

        // TODO: remove these
        ModLog.Info(Main.Randomizer.ItemHandler.CollectedItems.Count);
        ModLog.Info(__result.Result.doves);

        ModLog.Info($"Replacing slot info for slot {index}");
        bool isrando = Main.Randomizer.IsSlotLoaded;

        int doves = isrando ? GetDoves() : 0;
        __result.Result.doves = doves;

        Main.Randomizer.IsSlotLoaded = false;
    }

    private static int GetDoves()
    {
        int doves = 0;

        for (int i = 0; i < 5; i++)
        {
            if (Main.Randomizer.ItemHandler.IsItemCollected(Items.KEY_IDS[i]))
                doves |= (1 << i);
        }

        return doves;
    }
}

static class Items
{
    public static readonly string[] KEY_IDS =
    [
        "QI63", "QI64", "QI65", "QI66", "QI67"
    ];
}
