using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.DialogSystem;
using Il2CppTGK.Game.Managers;
using System;
using System.Linq;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Logs the id whenever a dialog is started
/// </summary>
//[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.ShowDialogWithObject))]
//class Dialog_Show_Patch
//{
//    public static void Prefix(Dialog dialog)
//    {
//        Main.Randomizer.Log("Showing dialog: " + dialog.name);
//    }
//}
//[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.ShowDialog), typeof(DialogID), typeof(DialogManager.DialogParameters))]
//class Dialog_ShowId_Patch
//{
//    public static void Prefix(DialogID dialogId)
//    {
//        Main.Randomizer.Log("Showing dialog: " + dialogId.name);
//    }
//}

/// <summary>
/// Perform handling on dialog objects when they are being shown
/// </summary>
[HarmonyPatch(typeof(DialogWindowLogic), nameof(DialogWindowLogic.ShowDialog))]
class DialogWindow_ShowNormal_Patch
{
    public static void Prefix(Dialog dialog)
    {
        Main.Randomizer.Log("Showing dialog: " + dialog.name);
    }
}
[HarmonyPatch(typeof(DialogWindowLogic), nameof(DialogWindowLogic.ShowQuestionDialog))]
class DialogWindow_ShowQuestion_Patch
{
    public static void Prefix(QuestionDialog dialog, ref bool disableFirstResponse)
    {
        Main.Randomizer.Log("Showing question dialog: " + dialog.name);

        if (handDialogs.Contains(dialog.name))
            disableFirstResponse = true;
    }

    private static readonly string[] handDialogs = new string[]
    {
            "DLG_QT_1201", "DLG_QT_1202", "DLG_QT_1203", "DLG_QT_1204"
    };
}

