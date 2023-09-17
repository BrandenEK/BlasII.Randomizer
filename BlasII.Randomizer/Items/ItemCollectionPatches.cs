using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;

namespace BlasII.Randomizer.Items
{
    // When an item interactable is picked up
    //[HarmonyPatch(typeof(Loot), nameof(Loot.GiveLoot))]
    //class Loot_Give_Patch
    //{
    //    public static bool Prefix(Loot __instance)
    //    {
    //        Main.Randomizer.LogError("Loot.GiveLoot");
    //        Main.Randomizer.LogError("Type: " + __instance.lootType.ToString());

    //        if (__instance.lootType != Loot.LootType.Item)
    //            return true;

    //        // FO tear/mark shrines, I need index in transform (i0.D2001) so maybe use lootinteractable.otherfunction ?

    //        string locationId = "ITEM_" + __instance.itemIdRef.LoadAsset().WaitForCompletion().name;
    //        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

    //        return false;
    //    }
    //}

    // When an item interactable is picked up
    [HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.UseLootByInteractor))]
    class LootInteractable_Use_Patch
    {
        public static void Prefix(LootInteractable __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.l{__instance.transform.GetSiblingIndex()}";
            Main.Randomizer.LogError("LootInteractable.UseLootByInteractor - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.loot = null;
            }
        }
    }

    // When an item if given through playermaker, such as dialog
    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class PlayMaker_AddItem_Patch
    {
        public static bool Prefix(AddItem __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.i{__instance.owner.transform.GetSiblingIndex()}";
            Main.Randomizer.LogError("AddItem.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // When a different reward if given through playermaker, such as boss defeat
    [HarmonyPatch(typeof(GiveReward), nameof(GiveReward.OnEnter))]
    class PlayMaker_GiveReward_Patch
    {
        public static bool Prefix(GiveReward __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.r{__instance.owner.transform.GetSiblingIndex()}";
            Main.Randomizer.LogError("GiveReward.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // When a weapon is unlocked
    [HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
    class PlayMaker_UnlockWeapon_Patch
    {
        public static bool Prefix(UnlockWeapon __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
            Main.Randomizer.LogError("UnlockWeapon.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // When a weapon is upgraded
    [HarmonyPatch(typeof(UpgradeWeaponTier), nameof(UpgradeWeaponTier.OnEnter))]
    class PlayMaker_UpgradeWeapon_Patch
    {
        public static bool Prefix(UpgradeWeaponTier __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
            Main.Randomizer.LogError("UpgradeWeaponTier.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // When an ability is unlocked
    [HarmonyPatch(typeof(UnlockAbility), nameof(UnlockAbility.OnEnter))]
    class PlayMaker_UnlockAbility_Patch
    {
        public static bool Prefix(UnlockAbility __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.a0";
            Main.Randomizer.LogError("UnlockAbility.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}