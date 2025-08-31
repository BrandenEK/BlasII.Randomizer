using BlasII.ModdingAPI;
using BlasII.Randomizer.Settings;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.PopupMessages;
using System.Linq;

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
            string text = Main.Randomizer.LocalizationHandler.Localize("popup/keys")
                .Replace("*", Main.Randomizer.CurrentSettings.RealRequiredKeys.ToString());
            __instance.textCtrl.SetText(text);
            return;
        }

        // When pressing the display button, show the current settings
        if (message.name == "TESTPOPUP")
        {
            string text = Main.Randomizer.CurrentSettings.FormatInfo();
            __instance.textCtrl.SetText(text);
            return;
        }
    }
}

/// <summary>
/// Skip display message when interacting with certain objects
/// </summary>
[HarmonyPatch(typeof(ShowPopupMessage), nameof(ShowPopupMessage.OnEnter))]
class ShowPopupMessage_OnEnter_Patch
{
    public static bool Prefix(ShowPopupMessage __instance)
    {
        string message = __instance.messageId?.name ?? "INVALID_id";
        
        if (!SKIPPED_MESSAGES.Contains(message))
            return true;

        ModLog.Info($"Skipping popup: {message}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] SKIPPED_MESSAGES =
    [
        "MSG_10101_id", // Mud key breaking
        "MSG_10102_id", // Mea Culpa Hilt loss
        "MSG_10103_id", // Mea Culpa Hilt retrieval
    ];
}
