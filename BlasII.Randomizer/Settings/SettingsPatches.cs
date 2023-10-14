using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    /// <summary>
    /// When pressing accept on a slot, open the settings menu instead
    /// When pressing accept on the settings menu, start the game
    /// </summary>
    [HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.NewGame))]
    class NewGame_Settings_Patch
    {
        public static bool Prefix(int slot)
        {
            if (NewGameFlag)
                return true;

            Main.Randomizer.SettingsHandler.OpenSettingsMenu(slot);
            return false;
        }

        public static void Postfix()
        {
            if (NewGameFlag)
                Main.Randomizer.NewGame();
        }

        public static bool NewGameFlag { get; set; }
    }

    /// <summary>
    /// Attempts to change the dimensions of the info popup to allow showing entire settings list
    /// </summary>
    [HarmonyPatch(typeof(PopupWindowLogic), nameof(PopupWindowLogic.ShowPopup))]
    class Popup_Show_Patch
    {
        public static void Postfix(PopupWindowLogic __instance)
        {
            Main.Randomizer.Log(__instance.PopUp.sizeDelta.y.ToString());
            Main.Randomizer.Log(__instance.descriptionText.preferredHeight.ToString());
            Main.Randomizer.Log(__instance.descriptionText.rectTransform.anchoredPosition.ToString());

            Main.Randomizer.Log(__instance.descriptionText.rectTransform.sizeDelta.ToString());

            //__instance.descriptionText.rectTransform.sizeDelta = new Vector2(540, 50);
            //float x = __instance.descriptionText.rectTransform.anchoredPosition.x - 20;
            //float y = __instance.descriptionText.rectTransform.anchoredPosition.y;
            //__instance.descriptionText.rectTransform.anchoredPosition = new Vector2(300, y);

            //float height = __instance.descriptionText.preferredHeight + 120;
            //__instance.PopUp.sizeDelta = new Vector2(__instance.PopUp.sizeDelta.x, height);
        }
    }
}
