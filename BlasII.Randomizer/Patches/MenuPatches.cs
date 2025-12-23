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
    public static void Postfix(SlotInfo info)
    {
        info.canConvertNGPlus = false;
    }
}

/// <summary>
/// Updates the slot UI with collected keys
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
/// Updates the slot UI with collected items
/// </summary>
[HarmonyPatch(typeof(UISelectableMainMenuSlot), nameof(UISelectableMainMenuSlot.SetSlotInfo))]
class UISelectableMainMenuSlot_SetSlotInfo_Patch
{
    public static void Postfix(UISelectableMainMenuSlot __instance, SlotInfo info)
    {
        int current = info.percentValue & 0xFFFF;
        int total = (info.percentValue >> 16) & 0xFFFF;
        string text = $"<voffset=10px>{current.ToString().PadLeft(total.ToString().Length)}<voffset=0px>/<voffset=-10px>{total}";

        __instance.gamePercent.SetText(text);
        __instance.gamePercent.shadowText.rectTransform.anchoredPosition = new UnityEngine.Vector2(-8, 0);
        __instance.gamePercent.normalText.rectTransform.anchoredPosition = new UnityEngine.Vector2(0, 4);
        __instance.gamePercent.shadowText.fontSize = 32;
        __instance.gamePercent.normalText.fontSize = 32;
        __instance.gamePercent.shadowText.m_lineHeight = 0.25f;
        __instance.gamePercent.normalText.m_lineHeight = 0.25f;
        __instance.gamePercent.shadowText.richText = true;
        __instance.gamePercent.normalText.richText = true;
    }
}

/// <summary>
/// Updates the SlotInfo with randomizer data
/// </summary>
[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.GetSlotInfo))]
class MainMenuWindowLogic_GetSlotInfo_Patch
{
    public static void Postfix(int index, Task<SlotInfo> __result)
    {
        ModLog.Info($"Replacing slot info for slot {index}");
        bool isrando = Main.Randomizer.IsSlotLoaded;

        __result.Result.doves = isrando ? GetDoves() : 0;
        __result.Result.percentValue = isrando ? GetItems() : 0;

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

    private static int GetItems()
    {
        int result = 0;

        result |= Main.Randomizer.ItemHandler.CollectedItemsDisplay;
        result |= (Main.Randomizer.ItemHandler.TotalItemsDisplay << 16);

        return result;
    }
}

static class Items
{
    public static readonly string[] KEY_IDS =
    [
        "QI63", "QI64", "QI65", "QI66", "QI67"
    ];
}
