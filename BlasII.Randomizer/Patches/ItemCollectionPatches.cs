using BlasII.ModdingAPI;
using BlasII.Randomizer.Extensions;
using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Achievements;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Managers;
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
        if (ROOMS_NEVER.Contains(sceneName) || ITEMS_NEVER.Contains(itemName) || COMBO_NEVER.Any(x => x.Item1 == sceneName && x.Item2 == itemName))
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
    private static readonly (string, string)[] COMBO_NEVER = [("Z2804", "QI104")]; // Mea Culpa Hilt after Asterion
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
class UnlockWeapon_OnEnter_Patch
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
class ShowWeaponPopup_OnEnter_Patch
{
    public static bool Prefix(ShowWeaponPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShopUnlockWeaponPopup), nameof(ShopUnlockWeaponPopup.OnEnter))]
class ShopUnlockWeaponPopup_OnEnter_Patch
{
    public static bool Prefix(ShopUnlockWeaponPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(SwapWeaponSlotsAction), nameof(SwapWeaponSlotsAction.OnEnter))]
class SwapWeaponSlotsAction_OnEnter_Patch
{
    public static bool Prefix(SwapWeaponSlotsAction __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        // TODO: Remove once I know they don't affect something else
        ModLog.Error("Skipping SwapWeaponSlotsAction");
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ChangeWeaponAction), nameof(ChangeWeaponAction.OnEnter))]
class ChangeWeaponAction_OnEnter_Patch
{
    public static bool Prefix(ChangeWeaponAction __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        // TODO: Remove once I know they don't affect something else
        ModLog.Error("Skipping ChangeWeaponAction");
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

[HarmonyPatch(typeof(AddProgressTokenComponent), nameof(AddProgressTokenComponent.AddProgressToken))]
class AddProgressTokenComponent_AddProgressToken_Patch
{
    public static bool Prefix(AddProgressTokenComponent __instance)
    {
        if (__instance.token.achievementId.name != "AC21")
            return true;

        string locationId = $"{CoreCache.Room.CurrentRoom.Name}.c0";
        ModLog.Custom($"AddProgressTokenComponent.AddProgressToken - {locationId}", System.Drawing.Color.Green);

        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        CoreCache.CherubsManager.Synch();
        return false;
    }
}
[HarmonyPatch(typeof(ShowCherubPopup), nameof(ShowCherubPopup.OnEnter))]
class ShowCherubPopup_OnEnter_Patch
{
    public static bool Prefix(ShowCherubPopup __instance)
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(CherubsManager), nameof(CherubsManager.Synch))]
class CherubsManager_Synch_Patch
{
    public static void Postfix()
    {
        if (!Main.Randomizer.IsRandomizerMode)
            return;

        int count = Main.Randomizer.ItemHandler.AmountItemCollected("CH");
        Main.Randomizer.SetQuestValue("ST16", "FREED_CHERUBS", count);
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
        if (!Main.Randomizer.IsRandomizerMode)
            return true;

        ModLog.Error("Hiding tears popup.  Not sure what else this affects.");
        __instance.Finish();
        return false;
    }
}