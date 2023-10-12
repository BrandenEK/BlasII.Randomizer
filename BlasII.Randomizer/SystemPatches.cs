using HarmonyLib;
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
}
