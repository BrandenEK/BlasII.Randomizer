using HarmonyLib;
using Il2CppTGK.Framework.Quest;
using Il2CppTGK.Game;

namespace BlasII.Randomizer
{
    //[HarmonyPatch(typeof(QuestVariable), nameof(QuestVariable.Set), typeof(string))]
    //class QuestSet_String_Patch
    //{
    //    public static void Postfix(QuestVariable __instance, string value)
    //    {
    //        if (Main.Randomizer.HasLeftMainMenu)
    //        {
    //            string quest = QuestGet_String_Patch.GetQuestFromVariable(__instance);
    //            Main.Randomizer.Log($"Setting quest {quest}.{__instance.id} = {value}");
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(QuestVariable), nameof(QuestVariable.Set), typeof(float))]
    //class QuestSet_Float_Patch
    //{
    //    public static void Postfix(QuestVariable __instance, float value)
    //    {
    //        if (Main.Randomizer.HasLeftMainMenu)
    //        {
    //            string quest = QuestGet_String_Patch.GetQuestFromVariable(__instance);
    //            Main.Randomizer.Log($"Setting quest {quest}.{__instance.id} = {value}");
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(QuestVariable), nameof(QuestVariable.Set), typeof(int))]
    //class QuestSet_Int_Patch
    //{
    //    public static void Postfix(QuestVariable __instance, int value)
    //    {
    //        if (Main.Randomizer.HasLeftMainMenu)
    //        {
    //            string quest = QuestGet_String_Patch.GetQuestFromVariable(__instance);
    //            Main.Randomizer.Log($"Setting quest {quest}.{__instance.id} = {value}");
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(QuestVariable), nameof(QuestVariable.Set), typeof(bool))]
    //class QuestSet_Bool_Patch
    //{
    //    public static void Postfix(QuestVariable __instance, bool value)
    //    {
    //        if (Main.Randomizer.HasLeftMainMenu)
    //        {
    //            string quest = QuestGet_String_Patch.GetQuestFromVariable(__instance);
    //            Main.Randomizer.Log($"Setting quest {quest}.{__instance.id} = {value}");
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(QuestVariable), nameof(QuestVariable.GetStringValue))]
    //class QuestGet_String_Patch
    //{
    //    public static void Postfix(QuestVariable __instance, string __result)
    //    {
    //        if (Main.Randomizer.HasLeftMainMenu)
    //        {
    //            string quest = GetQuestFromVariable(__instance);
    //            Main.Randomizer.Log($"Checking quest {quest}.{__instance.id} = {__result}");
    //        }
    //    }

    //    public static string GetQuestFromVariable(QuestVariable variable)
    //    {
    //        foreach (QuestDataInternal quest in CoreCache.Quest.quests.Values)
    //        {
    //            if (quest.currentStatus == variable || quest.vars.ContainsValue(variable))
    //                return quest.Name;
    //        }

    //        return "Unknown";
    //    }
    //}
}
