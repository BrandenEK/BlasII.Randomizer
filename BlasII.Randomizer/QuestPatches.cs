using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using System.Reflection;

namespace BlasII.Randomizer
{
    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
    class QuestManager_GetVarBool_Patch
    {
        public static void Postfix(int questId, int varId, ref bool __result)
        {
            Main.Randomizer.LogWarning($"Getting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({__result})");

            string scene = CoreCache.Room.CurrentRoom?.Name;
            string quest = Main.Randomizer.GetQuestName(questId, varId);

            // Always have zones unlocked
            if (quest.StartsWith("ST00.Z") && quest.EndsWith("_ACCESS"))
            {
                __result = true;
            }

            // Only unlock CR once all bosses are dead
            if (scene == "Z2501" && quest == "Bosses.BS07_DEAD")
            {
                __result = __result &&
                    Main.Randomizer.GetQuestBool("Bosses", "BS04_DEAD") &&
                    Main.Randomizer.GetQuestBool("Bosses", "BS05_DEAD") &&
                    Main.Randomizer.GetQuestBool("Bosses", "BS06_DEAD") &&
                    Main.Randomizer.GetQuestBool("Bosses", "BS08_DEAD");
            }
        }
    }

    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarIntValue))]
    class QuestManager_GetVarInt_Patch
    {
        public static void Postfix(int questId, int varId, ref int __result)
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
