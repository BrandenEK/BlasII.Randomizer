using BlasII.ModdingAPI;
using BlasII.Randomizer.Extensions;
using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;
using System.Linq;

namespace BlasII.Randomizer.Patches;

// ===============
// Item collection
// ===============

[HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.UseLootByInteractor))]
class LootInteractable_Use_Patch
{
    public static void Prefix(LootInteractable __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("LootInteractable.UseLootByInteractor - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.loot = null; // Can not simply return false
    }
}
[HarmonyPatch(typeof(LootInteractableST103), nameof(LootInteractableST103.UseLootByInteractor))]
class LootInteractableST103_Use_Patch
{
    public static bool Prefix(LootInteractableST103 __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("LootInteractableST103.UseLootByInteractor - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        // This is necessary to force the actual interaction to take place
        __instance.GetComponent<RoomInteractable>().Use();

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        return false;
    }
}
[HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
class PlayMaker_AddItem_Patch
{
    public static bool Prefix(AddItem __instance)
    {
        string sceneName = CoreCache.Room.CurrentRoom?.Name;
        string itemName = __instance.itemID.name;

        // Always give location if certain room or item
        if (ROOMS_ALWAYS.Contains(sceneName) || ITEMS_ALWAYS.Contains(itemName))
        {
            return true;
        }

        // Never give location if certain room or item
        if (ROOMS_NEVER.Contains(sceneName) || ITEMS_NEVER.Contains(itemName))
        {
            __instance.Finish();
            return false;
        }

        string locationId = __instance.CalculateId();
        ModLog.Custom($"AddItem.OnEnter - {locationId} ({itemName})", System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }

    private static readonly string[] ROOMS_ALWAYS = ["Z1809"]; // Ending A
    private static readonly string[] ITEMS_ALWAYS = ["FG40", "FG41", "FG42", "FG43"]; // Burnt figures
    private static readonly string[] ROOMS_NEVER = []; // None
    private static readonly string[] ITEMS_NEVER = ["QI102"]; // Broken key
}

// =================
// Defeat boss marks
// =================

[HarmonyPatch(typeof(GiveReward), nameof(GiveReward.OnEnter))]
class PlayMaker_GiveReward_Patch
{
    public static bool Prefix(GiveReward __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("GiveReward.OnEnter - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowOrbsRewardPopup), nameof(ShowOrbsRewardPopup.OnEnter))]
class Marks_Skip_Patch
{
    public static bool Prefix(ShowOrbsRewardPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}

// =====================
// Weapon unlock statues
// =====================

[HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
class PlayMaker_UnlockWeapon_Patch
{
    public static bool Prefix(UnlockWeapon __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("UnlockWeapon.OnEnter - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowWeaponPopup), nameof(ShowWeaponPopup.OnEnter))]
class WeaponFind_Skip_Patch
{
    public static bool Prefix(ShowWeaponPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}

// ======================
// Weapon upgrade statues
// ======================

[HarmonyPatch(typeof(UpgradeWeaponTier), nameof(UpgradeWeaponTier.OnEnter))]
class PlayMaker_UpgradeWeapon_Patch
{
    public static bool Prefix(UpgradeWeaponTier __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("UpgradeWeaponTier.OnEnter - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowWeaponTierPopup), nameof(ShowWeaponTierPopup.OnEnter))]
class WeaponUpgrade_Skip_Patch
{
    public static bool Prefix(ShowWeaponTierPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}

// ======================
// Ability unlock statues
// ======================

[HarmonyPatch(typeof(UnlockAbility), nameof(UnlockAbility.OnEnter))]
class PlayMaker_UnlockAbility_Patch
{
    public static bool Prefix(UnlockAbility __instance)
    {
        string locationId = __instance.CalculateId();
        ModLog.Custom("UnlockAbility.OnEnter - " + locationId, System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowUnlockAbilityPopup), nameof(ShowUnlockAbilityPopup.OnEnter))]
class Ability_Skip_Patch
{
    public static bool Prefix(ShowUnlockAbilityPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ActivateGoldFlaskAbilityAction), nameof(ActivateGoldFlaskAbilityAction.OnEnter))]
class ActivateGoldFlaskAbilityAction_OnEnter_Patch
{
    public static bool Prefix(ActivateGoldFlaskAbilityAction __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}

// =============
// Caged cherubs
// =============

// They changed how cherubs work an ruined everything.  The quest flags are still used in the cherub room and maybe on the pause menu.
// But now the CherubsManager stores collected cherubs as tokens and then syncs the quest flags.
// However, those tokens also determines whether the cherub is collected or not.
// In addition, the CherubCollectibleComponent.AddCherub method fires twice on scene load

//[HarmonyPatch(typeof(CherubCollectibleComponent), nameof(CherubCollectibleComponent.AddCherub))]
//class CherubCollectibleComponent_AddCherub_Patch
//{
//    public static bool Prefix()
//    {
//        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.c0";
//        **ModLog.Error("CherubCollectibleComponent.AddCherub - " + locationId);

//        if (!Main.Randomizer.IsRandomizerMode)
//            return true;

//        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
//        CoreCache.CherubsManager.Synch();
//        return false;
//    }
//}
//[HarmonyPatch(typeof(OperateQuestVar), nameof(OperateQuestVar.CheckInputData))]
//class PlayMaker_OperateQuestVar_Patch
//{
//    public static bool Prefix(OperateQuestVar __instance)
//    {
//        string quest = Main.Randomizer.GetQuestName(__instance.questVar.questID, __instance.questVar.varID);
//        if (quest != "ST16.FREED_CHERUBS")
//            return true;

//        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.c0";
//        **ModLog.Error("OperateQuestVar.CheckInputData - " + locationId);

//        if (!Main.Randomizer.IsRandomizerMode)
//            return true;

//        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
//        __instance.Finish();
//        return false;
//    }
//}
//[HarmonyPatch(typeof(ShowCherubPopup), nameof(ShowCherubPopup.OnEnter))]
//class Cherub_Skip_Patch
//{
//    public static bool Prefix(ShowCherubPopup __instance)
//    {
//        if (!Main.Randomizer.IsRandomizerMode)
//            return true;

//        __instance.Finish();
//        return false;
//    }
//}

// =====
// Tears
// =====

[HarmonyPatch(typeof(ShowTearsRewardPopup), nameof(ShowTearsRewardPopup.OnEnter))]
class Tears_Show_Patch
{
    public static bool Prefix(ShowTearsRewardPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        ModLog.Error("Hiding tears popup.  Not sure what else this affects.");
        __instance.Finish();
        return false;
    }
}