using BlasII.ModdingAPI;
using BlasII.Randomizer.Settings;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PopupMessages;
using System.Linq;
using UnityEngine;

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

        // When pressing display button, show the current settings
        if (message.name == "TESTPOPUP")
        {
            string text = Main.Randomizer.CurrentSettings.FormatInfo();
            __instance.textCtrl.SetText(text);
            __instance.OnClose();
            return;
        }

        // When talking to cobijada mother, dont display upgrade type
        if (message.name == "MSG_2501" ||
            message.name == "MSG_2502" ||
            message.name == "MSG_2503")
        {
            string text = Main.Randomizer.LocalizationHandler.Localize("popup/sisters");
            __instance.textCtrl.SetText(text);
            return;
        }

        // When opening a mud door, dont show the removal
        if (message.name == "MSG_10101")
        {
            string text = Main.Randomizer.LocalizationHandler.Localize("popup/mud");
            __instance.textCtrl.SetText(text);
            return;
        }

        // When defeating Asterion, dont show retrieving the hilt
        if (message.name == "MSG_10103")
        {
            string text = Main.Randomizer.LocalizationHandler.Localize("popup/boss");
            __instance.textCtrl.SetText(text);
            return;
        }
    }
}

/// <summary>
/// Remove display message when interacting with certain objects
/// </summary>
[HarmonyPatch(typeof(ShowPopupMessage), nameof(ShowPopupMessage.OnEnter))]
class ShowPopupMessage_OnEnter_Patch
{
    public static bool Prefix(ShowPopupMessage __instance)
    {
        string message = __instance.messageId?.name ?? "INVALID_id";
        
        // TODO: remove this line
        ModLog.Error($"Trying to show popup: {message}");

        if (!SKIPPED_MESSAGES.Contains(message))
            return true;

        ModLog.Info($"Skipping popup: {message}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] SKIPPED_MESSAGES =
    [
        "MSG_10101_id", // Mud key breaking
        // Mea Culpa Hilt loss
        "MSG_10103_id", // Mea Culpa Hilt retrieval
    ];
}


[HarmonyPatch(typeof(UINavigationHelper), nameof(UINavigationHelper.ShowPopupMessage), typeof(PopupMessageID), typeof(bool))]
class UINavigationHelper_ShowPopupMessage_Patch
{
    public static bool Prefix(PopupMessageID popupMessageID)
    {
        //ModLog.Error($"Trying to show popup: {popupMessageID.name}");

        //MSG_10103_id
        //if (popupMessageID.name == "TESTPOPUP_id")
        //    return false;

        return true;
    }
}

/// <summary>
/// Adjust the size of the item popup based on the image
/// </summary>
[HarmonyPatch(typeof(ItemPopupWindowLogic), nameof(ItemPopupWindowLogic.ShowPopupInternal))]
class ItemPopupWindowLogic_ShowPopupInternal_Patch
{
    public static void Prefix(ItemPopupWindowLogic __instance, ItemPopupWindowLogic.ItemPopUpData popup)
    {
        Vector2 size = popup.image == null ? new Vector2(90, 90) : popup.image.rect.size * 3;
        Vector2 offset = new((90 - size.x) / 2, (90 - size.y) / 2);

        __instance.spriteImage.rectTransform.sizeDelta = size;
        __instance.spriteImage.rectTransform.anchoredPosition = offset;
    }
}
