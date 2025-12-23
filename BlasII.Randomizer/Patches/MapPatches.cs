using HarmonyLib;
using Il2CppI2.Loc;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// When opening the map, replace the cherub count with collected items count
/// </summary>
[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.UpdateUIElements))]
class MapWindowLogic_UpdateUIElements_Patch
{
    public static void Postfix(MapWindowLogic __instance)
    {
        Transform cherubHolder = __instance.transform.Find("Background/UpperZone/PercentMapAndCherubs/cherubs");
        if (cherubHolder == null)
            return;

        int currentItems = Main.Randomizer.ItemHandler.CollectedItemsDisplay;
        int totalItems = Main.Randomizer.ItemHandler.TotalItemsDisplay;

        var leftText = cherubHolder.GetChild(0).GetChild(0).GetComponent<UIPixelTextWithShadow>();
        leftText.SetText(currentItems.ToString());
        Object.Destroy(leftText.GetComponent<Localize>());

        var rightText = cherubHolder.GetChild(0).GetChild(1).GetComponent<UIPixelTextWithShadow>();
        rightText.SetText("/" + totalItems);
        Object.Destroy(rightText.GetComponent<Localize>());

        var image = cherubHolder.GetChild(1).Cast<RectTransform>();
        image.anchoredPosition = new Vector2(30, 0);
        image.sizeDelta = new Vector2(80, 80);
        image.GetComponent<Image>().sprite = Main.Randomizer.CustomIconStorage.GetImage(Storages.CustomIconStorage.IconType.Chest);
    }
}
