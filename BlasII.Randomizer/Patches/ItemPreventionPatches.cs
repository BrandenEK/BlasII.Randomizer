using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Managers;

namespace BlasII.Randomizer.Patches;

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

        ModLog.Warn($"{__instance.Owner.name} is checking for item: {item}");

        // Cursed letter quest
        if (scene == "Z1326" && (item == "PR15" || item == "QI15" || item == "QI16") ||
            scene == "Z0502" && item == "PR15" ||
            scene == "Z0503" && item == "PR15" || // The rest of check is in other patch
            scene == "Z1917" && item == "PR15")
        {
            RunEvent(__instance, false);
            return false;
        }

        // Lullaby quest
        if (scene == "Z1906" && item == "PR16")
        {
            RunEvent(__instance, false);
            return false;
        }

        // Chime symbol quest
        if (scene == "Z1421" && item == "PR03")
        {
            RunEvent(__instance, Main.Randomizer.ItemHandler.IsLocationCollected("Z1421.l1"));
            return false;
        }

        // Incense quest
        if (scene == "Z1064" && item == "QI69")
        {
            RunEvent(__instance, Main.Randomizer.ItemHandler.IsLocationCollected("Z1064.i0"));
            return false;
        }

        return true;
    }

    private static void RunEvent(IsItemOwned action, bool hasItem)
    {
        action.Fsm.Event(hasItem ? action.yesEvent : action.noEvent);
        action.Finish();
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

        ModLog.Warn($"{__instance.Owner.name} is checking for items: {items}");

        // Cursed letter quest again
        if (scene == "Z0503" && firstItem == "QI21")
        {
            RunEvent(__instance, false);
            return false;
        }

        return true;
    }

    private static void RunEvent(AreAnyItemOwned action, bool hasItem)
    {
        action.Fsm.Event(hasItem ? action.yesEvent : action.noEvent);
        action.Finish();
    }
}
[HarmonyPatch(typeof(RemoveItem), nameof(RemoveItem.OnEnter))]
class RemoveItem_OnEnter_Patch
{
    public static bool Prefix(RemoveItem __instance)
    {
        string scene = CoreCache.Room.CurrentRoom?.Name;
        string item = __instance.itemID.name;

        ModLog.Warn($"{__instance.Owner.name} is trying to remove item: {item}");

        // Mud quest
        if (item == "QI101")
        {
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

        ModLog.Warn($"{__instance.Owner.name} is checking for weapon: {weapon}");

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
    [HarmonyPriority(Priority.High)]
    public static void Postfix(int questId, int varId, ref bool __result)
    {
        string scene = CoreCache.Room.CurrentRoom?.Name;
        string quest = Main.Randomizer.GetQuestName(questId, varId);

        // Always have zones unlocked
        if (quest.StartsWith("ST00.Z") && quest.EndsWith("_ACCESS"))
        {
            __result = true;
        }

        // Always have ET unlocked
        else if (scene == "Z0504" && quest.StartsWith("Bosses"))
        {
            __result = true;
        }

        // Always have sand emptied
        else if (quest.StartsWith("Z04"))
        {
            __result = true;
        }

        // Only unlock CR once enough keys are owned
        else if (scene == "Z2501" && quest == "Bosses.BS07_DEAD")
        {
            __result = OwnedKeys >= Main.Randomizer.CurrentSettings.RealRequiredKeys;
            ModLog.Info("CR door opened: " + __result);
        }

        // Allow giving both the scroll & cloth to the elders
        else if (quest == "ST03.TRIFON_DEAD" || quest == "ST03.CAYA_DEAD")
        {
            __result = false;
        }

        // Always allow the hand to upgrade fervour
        else if (scene == "Z1708" && quest == "ST12.HAND_SECRET")
        {
            __result = false;
        }
    }

    private static int OwnedKeys
    {
        get
        {
            int keys = 0;
            if (AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI63"])) keys++;
            if (AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI64"])) keys++;
            if (AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI65"])) keys++;
            if (AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI66"])) keys++;
            if (AssetStorage.PlayerInventory.HasItem(AssetStorage.QuestItems["QI67"])) keys++;
            return keys;
        }
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarIntValue))]
class QuestManager_GetVarInt_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Postfix(int questId, int varId, ref int __result)
    {
        string scene = CoreCache.Room.CurrentRoom?.Name;
        string quest = Main.Randomizer.GetQuestName(questId, varId);

        // No checks yet
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarValue))]
class QuestManager_GetVar_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Postfix(int questId, int varId, ref string __result)
    {
        string scene = CoreCache.Room.CurrentRoom?.Name;
        string quest = Main.Randomizer.GetQuestName(questId, varId);

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

/// <summary>
/// Prevent the GoldenLumpsCountFix from functioning
/// </summary>
[HarmonyPatch(typeof(GoldenLumpsCountFix), nameof(GoldenLumpsCountFix.PlayerSpawnManager_OnPlayerSpawned))]
class GoldenLumpsCountFix_OnPlayerSpawned_Patch
{
    public static bool Prefix()
    {
        ModLog.Info("Preventing gold lump sync");
        return false;
    }
}
