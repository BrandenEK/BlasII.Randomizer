using HarmonyLib;
using Il2CppTGK.Game.Components.Misc;
using Il2CppTGK.Game.Managers;
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
}
