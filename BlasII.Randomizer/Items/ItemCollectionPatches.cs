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

            Main.Randomizer.LogError("Loot.GiveLoot " + __instance.itemIdRef);
            return true;
        }
    }

    // When an item if given through playermaker, such as dialog
    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class PlayerMaker_AddItem_Patch
    {
        public static bool Prefix(AddItem __instance)
        {
            Main.Randomizer.LogError("AddItem.OnEnter: " + __instance.itemID.name);
            return true;
        }
    }

    // When a weapon is unlocked
    [HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
    class PlayerMaker_UnlockWeapon_Patch
    {
        public static bool Prefix(UnlockWeapon __instance)
        {
            Main.Randomizer.LogError("UnlockWeapon.OnEnter: " + __instance.weaponID.Value.Cast<WeaponID>().name);
            return true;
        }
    }

    // When an ability is unlocked
    [HarmonyPatch(typeof(UnlockAbility), nameof(UnlockAbility.OnEnter))]
    class PlayMaker_UnlockAbility_Patch
    {
        public static bool Prefix(UnlockAbility __instance)
        {
            Main.Randomizer.LogError("UnlockAbility.OnEnter: " + __instance.AbilityTypeRef.Value.Cast<IAbilityTypeRef>().name);
            return true;
        }
    }
}