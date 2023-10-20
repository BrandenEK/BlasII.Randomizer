using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.PopupMessages;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer
{
    /// <summary>
    /// When reading the CR door, show how many keys you must find
    /// When pressing display button, show the current settings
    /// </summary>
    [HarmonyPatch(typeof(PopupMessageLogic), nameof(PopupMessageLogic.ShowMessageAndWait))]
    class Popup_Show_Patch
    {
        public static void Postfix(PopupMessageLogic __instance, PopupMessage message)
        {
            Main.Randomizer.Log("Showing popup: " + message.name);

            if (message.name == "MSG_0003")
            {
                string text = Main.Randomizer.LocalizationHandler.Localize("ktex")
                    .Replace("*", Main.Randomizer.CurrentSettings.RealRequiredKeys.ToString());
                __instance.textCtrl.SetText(text);
                return;
            }

            if (message.name == "TESTPOPUP")
            {
                string text = Main.Randomizer.CurrentSettings.FormatInfo();
                __instance.textCtrl.SetText(text);
                return;
            }
        }
    }

    /// <summary>
    /// When opening the map, replace the cherub count with collected items count
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnShow))]
    class Map_Show_Patch
    {
        public static void Postfix(MapWindowLogic __instance)
        {
            Transform cherubHolder = __instance.transform.Find("Background/UpperZone/PercentMapAndCherubs/cherubs");
            if (cherubHolder == null)
                return;

            cherubHolder.GetChild(0).GetChild(0).GetComponent<UIPixelTextWithShadow>().SetText("0");
            cherubHolder.GetChild(0).GetChild(1).GetComponent<UIPixelTextWithShadow>().SetText("/255");

            Main.Randomizer.Log(cherubHolder.GetChild(1).GetComponent<Image>().sprite.pixelsPerUnit);
        }
    }
}
