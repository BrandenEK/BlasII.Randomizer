using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PlayerSpawn;
using System.Collections.Generic;

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

            // Cursed letter quest
            if (scene == "Z1326" && (item == "PR15" || item == "QI15" || item == "QI16") ||
                scene == "Z0502" && item == "PR15" ||
                scene == "Z0503" && item == "PR15" || // The rest of check is in other patch
                scene == "Z1917" && item == "PR15")
            {
                __instance.Fsm.Event(__instance.noEvent);
                __instance.Finish();
                return false;
            }

            // Lullaby quest
            if (scene == "Z1906" && item == "PR16")
            {
                __instance.Fsm.Event(__instance.noEvent);
                __instance.Finish();
                return false;
            }

            return true;
        }
    }
    [HarmonyPatch(typeof(AreAnyItemOwned), nameof(AreAnyItemOwned.OnEnter))]
    class Check_ItemsOwned_Patch
    {
        public static bool Prefix(AreAnyItemOwned __instance)
        {
            string scene = CoreCache.Room.CurrentRoom?.Name;
            string firstItem = __instance.items[0].name;
            string items = "";
            foreach (var item in __instance.items)
                items += item.name + " ";

            Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking for items: {items}");

            // Cursed letter quest again
            if (scene == "Z0503" && firstItem == "QI21")
            {
                __instance.Fsm.Event(__instance.noEvent);
                __instance.Finish();
                return false;
            }

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
            bool initialResult = __result;

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

            // Only unlock CR once enough keys are owned
            else if (scene == "Z2501" && quest == "Bosses.BS07_DEAD")
            {
                __result = OwnedKeys >= Main.Randomizer.CurrentSettings.RealRequiredKeys;
                Main.Randomizer.Log("CR door opened: " + __result);
            }

            // Allow giving both the scroll & cloth to the elders
            else if (quest == "ST03.TRIFON_DEAD" || quest == "ST03.CAYA_DEAD")
            {
                __result = false;
            }

            if (!quest.StartsWith("ST18"))
                Main.Randomizer.LogWarning($"Getting quest: {quest} ({initialResult}) -> ({__result})");
        }

        private static int OwnedKeys
        {
            get
            {
                int keys = 0;
                if (ItemStorage.TryGetQuestItem("QI63", out var key1) && ItemStorage.PlayerInventory.HasItem(key1)) keys++;
                if (ItemStorage.TryGetQuestItem("QI64", out var key2) && ItemStorage.PlayerInventory.HasItem(key2)) keys++;
                if (ItemStorage.TryGetQuestItem("QI65", out var key3) && ItemStorage.PlayerInventory.HasItem(key3)) keys++;
                if (ItemStorage.TryGetQuestItem("QI66", out var key4) && ItemStorage.PlayerInventory.HasItem(key4)) keys++;
                if (ItemStorage.TryGetQuestItem("QI67", out var key5) && ItemStorage.PlayerInventory.HasItem(key5)) keys++;
                return keys;
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

    // ==========
    // Boss rooms
    // ==========

    [HarmonyPatch(typeof(PlayerSpawnManager), nameof(PlayerSpawnManager.TeleportPlayer))]
    class Teleport_Dream_Patch
    {
        public static void Prefix(ref SceneEntryID sceneEntry)
        {
            Main.Randomizer.Log($"Teleporting to: {sceneEntry.scene} ({sceneEntry.entryId})");

            string currentScene = CoreCache.Room.CurrentRoom?.Name;

            if (sceneEntry.scene.StartsWith("Z15") && bossRooms.TryGetValue(currentScene, out int entry))
            {
                Main.Randomizer.SetQuestValue("ST00", "DREAM_RETURN", true);
                sceneEntry = new SceneEntryID()
                {
                    scene = currentScene == "Z1113" ? "Z1104" : currentScene,
                    entryId = entry
                };
            }
        }

        private static readonly Dictionary<string, int> bossRooms = new()
        {
            { "Z0421", 685874534 },
            { "Z0730", 685874534 },
            { "Z0921", -777454601 },
            { "Z2304", 1157051513 },
            { "Z1113", 1928462977 },
            { "Z1216", -1794395 },
            { "Z1622", 887137572 },
            { "Z1327", -284092948 },
            // Z2501: -784211135
        };
    }
}
