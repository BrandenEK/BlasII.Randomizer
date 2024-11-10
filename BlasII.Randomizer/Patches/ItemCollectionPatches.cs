using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.PlayMaker.Actions;

namespace BlasII.Randomizer.Patches;

// ===============
// Item collection
// ===============

[HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.UseLootByInteractor))]
class LootInteractable_Use_Patch
{
    public static void Prefix(LootInteractable __instance)
    {
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.l{__instance.transform.GetSiblingIndex()}";
        ModLog.Error("LootInteractable.UseLootByInteractor - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
            return;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.loot = null;
    }
}
[HarmonyPatch(typeof(LootInteractableST103), nameof(LootInteractableST103.UseLootByInteractor))]
class LootInteractableST103_Use_Patch
{
    public static bool Prefix(LootInteractableST103 __instance)
    {
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.l{__instance.transform.GetSiblingIndex()}";
        ModLog.Error("LootInteractableST103.UseLootByInteractor - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        return false;
    }
}
[HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
class PlayMaker_AddItem_Patch
{
    public static bool Prefix(AddItem __instance)
    {
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.i{__instance.owner.transform.GetSiblingIndex()}";
        locationId = CalculateSpecialLocationId(locationId, __instance.itemID.name);
        ModLog.Error($"AddItem.OnEnter - {locationId} ({__instance.itemID.name})");

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }

    private static string CalculateSpecialLocationId(string locationId, string originalItem)
    {
        // Battle arenas
        if (locationId.StartsWith("Z15"))
        {
            return originalItem switch
            {
                "FG27" => "Z1501.s0",
                "FG28" => "Z1501.s1",
                "QI49" => "Z1501.s2",
                "QI46" => "Z1501.s3",
                "FG32" => "Z1501.s4",
                _ => string.Empty
            };
        }

        // Palace elders
        if (locationId.StartsWith("Z0722"))
        {
            return originalItem switch
            {
                "QI09" => "Z0722.s0",
                "QI10" => "Z0722.s1",
                _ => string.Empty
            };
        }

        return locationId;
    }
}

// =================
// Defeat boss marks
// =================

[HarmonyPatch(typeof(GiveReward), nameof(GiveReward.OnEnter))]
class PlayMaker_GiveReward_Patch
{
    public static bool Prefix(GiveReward __instance)
    {
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.r{__instance.owner.transform.GetSiblingIndex()}";
        ModLog.Error("GiveReward.OnEnter - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
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
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
        ModLog.Error("UnlockWeapon.OnEnter - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
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
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
        ModLog.Error("UpgradeWeaponTier.OnEnter - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
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
        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.a0";
        ModLog.Error("UnlockAbility.OnEnter - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
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
        __instance.Finish();
        return false;
    }
}

// =============
// Caged cherubs
// =============

[HarmonyPatch(typeof(OperateQuestVar), nameof(OperateQuestVar.CheckInputData))]
class PlayMaker_OperateQuestVar_Patch
{
    public static bool Prefix(OperateQuestVar __instance)
    {
        string quest = Main.Randomizer.GetQuestName(__instance.questVar.questID, __instance.questVar.varID);
        if (quest != "ST16.FREED_CHERUBS")
            return true;

        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.c0";
        ModLog.Error("OperateQuestVar.CheckInputData - " + locationId);

        if (Main.Randomizer.ItemHandler.IsVanillaLocation(locationId))
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowCherubPopup), nameof(ShowCherubPopup.OnEnter))]
class Cherub_Skip_Patch
{
    public static bool Prefix(ShowCherubPopup __instance)
    {
        __instance.Finish();
        return false;
    }
}

// =====
// Tears
// =====

[HarmonyPatch(typeof(ShowTearsRewardPopup), nameof(ShowTearsRewardPopup.OnEnter))]
class Tears_Show_Patch
{
    public static bool Prefix(ShowTearsRewardPopup __instance)
    {
        ModLog.Error("Hiding tears popup.  Not sure what else this affects.");
        __instance.Finish();
        return false;
    }
}