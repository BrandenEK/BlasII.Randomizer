using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.PlayMaker.Actions;
using System.Reflection;

namespace BlasII.Randomizer.Items
{
    //[HarmonyPatch(typeof(IsItemOwned), nameof(IsItemOwned.OnEnter))]
    //class Check_ItemOwned_Patch
    //{
    //    public static void Postfix(IsItemOwned __instance)
    //    {
    //        Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking for item: {__instance.itemID.name}");
    //    }
    //}

    //[HarmonyPatch(typeof(SetQuestVar), nameof(SetQuestVar.OnEnter))]
    //class Quest_Set_Patch
    //{
    //    public static void Postfix(SetQuestVar __instance)
    //    {
    //        Main.Randomizer.LogWarning($"{__instance.Owner.name} is setting quest: {__instance.questVar?.questID}.{__instance.questVar?.varID}");
    //    }
    //}

    //[HarmonyPatch(typeof(GetQuestVar), nameof(GetQuestVar.OnEnter))]
    //class Quest_Get_Patch
    //{
    //    public static void Postfix(GetQuestVar __instance)
    //    {
    //        Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking quest: {__instance.questId.Value}.{__instance.varId.Value}");
    //    }
    //}

    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarValue))]
    class QuestManager_GetVar_Patch
    {
        public static void Postfix(int questId, int varId)
        {
            Main.Randomizer.Log($"Getting quest: old version");
        }
    }

    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
    class QuestManager_GetVarBool_Patch
    {
        public static void Postfix(int questId, int varId, bool __result)
        {
            Main.Randomizer.Log($"Getting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({__result})");
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
            Main.Randomizer.Log($"Setting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({value})");
        }
    }

    //[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.SetQuestVarValue))]
    //class QuestManager_SetVar_Patch
    //{
    //    public static void Postfix(int questId, int varId)
    //    {
    //        Main.Randomizer.Log($"Setting quest: {questId}.{varId}");
    //    }
    //}
    // Patch quest manager gets instead
    // Most things call for the input var, then use the ids from that to call the quest manager ? maybe
}
