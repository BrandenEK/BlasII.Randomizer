using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.StatsSystem;
using Il2CppTGK.Game.Components.StatsSystem.Data;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Loot;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;

namespace BlasII.Randomizer.Items
{
    //[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
    //class test2
    //{
    //    public static void Postfix(ref ItemID itemID)
    //    {
    //        Main.Randomizer.LogWarning("InventoryComponent.AddItemAsync: " + itemID.name);
    //    }
    //}

    //[HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.GiveLoot))]
    //class ntest
    //{
    //    public static void Prefix()
    //    {
    //        Main.Randomizer.LogError("Giving loot");
    //    }
    //    public static void Postfix()
    //    {
    //        Main.Randomizer.LogError("Giving loot");
    //    }
    //}

    // When an item interactable is picked up
    [HarmonyPatch(typeof(Loot), nameof(Loot.GiveLoot))]
    class Loot_Give_Patch
    {
        public static bool Prefix(Loot __instance)
        {
            if (__instance.lootType != Loot.LootType.Item)
                return true;

            Main.Randomizer.LogError("Loot.GiveLoot");

            string locationId = "ITEM_???"; // + __instance.itemIdRef.LoadAsset().Result.name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
            return true;
        }
    }

    // When an item if given through playermaker, such as dialog
    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class PlayerMaker_AddItem_Patch
    {
        public static bool Prefix(AddItem __instance)
        {
            Main.Randomizer.LogError("AddItem.OnEnter");

            string locationId = "ITEM_" + __instance.itemID.name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
            return true;
        }
    }

    // When a weapon is unlocked
    [HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
    class PlayerMaker_UnlockWeapon_Patch
    {
        public static bool Prefix(UnlockWeapon __instance)
        {
            Main.Randomizer.LogError("UnlockWeapon.OnEnter");

            string locationId = "WEAPON_" + __instance.weaponID.Value.Cast<WeaponID>().name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
            return true;
        }
    }

    // When an ability is unlocked
    [HarmonyPatch(typeof(UnlockAbility), nameof(UnlockAbility.OnEnter))]
    class PlayMaker_UnlockAbility_Patch
    {
        public static bool Prefix(UnlockAbility __instance)
        {
            Main.Randomizer.LogError("UnlockAbility.OnEnter");

            string locationId = "ABILITY_" + __instance.AbilityTypeRef.Value.Cast<IAbilityTypeRef>().name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
            return true;
        }
    }
}