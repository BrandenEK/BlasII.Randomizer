using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.PopupMessages;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Change display message when interacting with certain objects
/// </summary>
[HarmonyPatch(typeof(PopupMessageLogic), nameof(PopupMessageLogic.ShowMessageAndWait))]
class PopupMessageLogic_ShowMessageAndWait_Patch
{
    public static void Postfix(PopupMessageLogic __instance, PopupMessage message)
    {
        ModLog.Info("Showing popup: " + message.name);

        // When reading the CR door, show how many keys you must find
        if (message.name == "MSG_0003")
        {
            string text = Main.Randomizer.LocalizationHandler.Localize("ktex")
                .Replace("*", Main.Randomizer.CurrentSettings.RealRequiredKeys.ToString());
            __instance.textCtrl.SetText(text);
            return;
        }

        // When pressing display button, show the current settings
        if (message.name == "TESTPOPUP")
        {
            string text = Main.Randomizer.CurrentSettings.FormatInfo();
            __instance.textCtrl.SetText(text);
            return;
        }

        // When talking to cobijada mother, dont display upgrade type
        if (message.name == "MSG_2501" ||
            message.name == "MSG_2502" ||
            message.name == "MSG_2503")
        {
            string text = "...";
            __instance.textCtrl.SetText(text);
            return;
        }
    }
}
