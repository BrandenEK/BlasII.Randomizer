using HarmonyLib;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.Misc;
using Il2CppTGK.Game.DialogSystem;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;
using System.Reflection;

namespace BlasII.Randomizer
{
    /// <summary>
    /// Log when a quest flag is being set
    /// </summary>
    [HarmonyPatch]
    class QuestManager_SetQuestBool_Patch
    {
        public static MethodInfo TargetMethod()
        {
            return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(bool));
        }

        public static void Postfix(int questId, int varId, bool value)
        {
            Main.Randomizer.LogWarning($"Setting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({value})");
        }
    }

    /// <summary>
    /// Prevent this class from hiding the cursor, even if cursor.visible = true
    /// </summary>
    [HarmonyPatch(typeof(MouseCursorVisibilityController), nameof(MouseCursorVisibilityController.Update))]
    class Mouse_Update_Patch
    {
        public static bool Prefix() => false;
    }
    [HarmonyPatch(typeof(MouseCursorVisibilityController), nameof(MouseCursorVisibilityController.Awake))]
    class Mouse_Awake_Patch
    {
        public static bool Prefix() => false;
    }

    /// <summary>
    /// Logs the id whenever a dialog is started
    /// </summary>
    [HarmonyPatch(typeof(DialogManager), nameof(DialogManager.ShowDialogWithObject))]
    class Dialog_Show_Patch
    {
        public static void Prefix(Dialog dialog)
        {
            Main.Randomizer.Log("Starting dialog: " + dialog.name);
        }
    }

    /// <summary>
    /// Always allow upgrading weapons, even without lance
    /// </summary>
    [HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.HasItem))]
    class Inventory_HasItem_Patch
    {
        public static void Postfix(ItemID itemID, ref bool __result)
        {
            __result = __result || itemID.name == "QI70";
        }
    }
}
