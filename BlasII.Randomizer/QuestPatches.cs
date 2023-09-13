using HarmonyLib;
using Il2CppTGK.Game.Managers;
using System.Reflection;

namespace BlasII.Randomizer
{
    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
    class QuestManager_GetVarBool_Patch
    {
        public static void Postfix(int questId, int varId, bool __result)
        {
            Main.Randomizer.LogWarning($"Getting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({__result})");
        }
    }

    [HarmonyPatch]
    class QuestManager_SetQuest_Patch
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
