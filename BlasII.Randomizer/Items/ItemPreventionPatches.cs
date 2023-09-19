using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Managers;

namespace BlasII.Randomizer.Items
{
    // =====
    // Items
    // =====

    [HarmonyPatch(typeof(IsItemOwned), nameof(IsItemOwned.OnEnter))]
    class Check_ItemOwned_Patch
    {
        public static bool Prefix(IsItemOwned __instance)
        {
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string item = __instance.itemID.name;

            Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking for item: {item}");

            // No checks yet

            return true;
        }
    }

    // =======
    // Weapons
    // =======

    [HarmonyPatch(typeof(IsWeaponUnlocked), nameof(IsWeaponUnlocked.OnEnter))]
    class Check_WeaponUnlocked_Patch
    {
        public static bool Prefix(IsWeaponUnlocked __instance)
        {
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string weapon = __instance.weaponID.Value.Cast<WeaponID>().name;

            Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking for weapon: {weapon}");

            // In weapon statue rooms, always make statues be destroyed
            if (scene == "Z0423" || scene == "Z0709" || scene == "Z0913")
            {
                __instance.Fsm.Event(__instance.isUnlocked);
                __instance.Finish();
                return false;
            }

            return true;
        }
    }

    // ======
    // Quests
    // ======

    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
    class QuestManager_GetVarBool_Patch
    {
        public static void Postfix(int questId, int varId, ref bool __result)
        {
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string quest = Main.Randomizer.GetQuestName(questId, varId);

            Main.Randomizer.LogWarning($"Getting quest: {quest} ({__result})");

            // Always have zones unlocked
            if (quest.StartsWith("ST00.Z") && quest.EndsWith("_ACCESS"))
            {
                __result = true;
            }

            // Always have sand emptied
            else if (quest.StartsWith("Z04"))
            {
                __result = true;
            }

            // Skip all tutorials
            else if (quest.StartsWith("Tutorials"))
            {
                __result = true;
            }

            // Skip all boss intros
            else if (quest.StartsWith("BossesIntro"))
            {
                __result = true;
            }

            // Only unlock CR once all bosses are dead
            else if (scene == "Z2501" && quest == "Bosses.BS07_DEAD")
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
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string quest = Main.Randomizer.GetQuestName(questId, varId);

            Main.Randomizer.LogWarning($"Getting quest: {quest} ({__result})");

            // No checks yet
        }
    }

    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarValue))]
    class QuestManager_GetVar_Patch
    {
        public static void Postfix(int questId, int varId, ref string __result)
        {
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string quest = Main.Randomizer.GetQuestName(questId, varId);

            Main.Randomizer.LogWarning($"Getting quest: {quest} (\"{__result}\")");

            // When checking for initial weapon in statue rooms, always make it seem like that is your first weapon
            if (quest == "ST00.WEAPON_CHOSEN")
            {
                if (scene == "Z0423")
                {
                    __result = "WEAPON_CENSER";
                }
                else if (scene == "Z0709")
                {
                    __result = "WEAPON_RAPIER";
                }
                else if (scene == "Z0913")
                {
                    __result = "WEAPON_ROSARY";
                }
            }
        }
    }
}
