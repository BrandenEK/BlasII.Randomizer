using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
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
    [HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
    class test2
    {
        public static void Postfix(ref ItemID itemID)
        {
            Main.Randomizer.LogWarning("InventoryComponent.AddItemAsync: " + itemID.name);
        }
    }

    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class test
    {
        public static void Postfix(AddItem __instance)
        {
            Main.Randomizer.LogError("AddItem.OnEnter: " + __instance.itemID.name);
        }
    }

    [HarmonyPatch(typeof(AbilityLockManager), nameof(AbilityLockManager.SetAbility))]
    class test5
    {
        public static void Postfix(IAbilityTypeRef abilityID)
        {
            Main.Randomizer.LogError("AbilityLockManager.SetAbility: " + abilityID.name);
        }
    }

    [HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
    class test6
    {
        public static void Postfix(UnlockWeapon __instance)
        {
            Main.Randomizer.LogError("UnlockWeapon.OnEnter: " + __instance.weaponID.Value.Cast<WeaponID>().name);
        }
    }

    [HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.GiveLoot))]
    class ntest
    {
        public static void Prefix()
        {
            Main.Randomizer.LogError("Giving loot");
        }
        public static void Postfix()
        {
            Main.Randomizer.LogError("Giving loot");
        }
    }

    [HarmonyPatch(typeof(Loot), nameof(Loot.GiveLoot))]
    class n2test
    {
        public static bool Prefix(Loot __instance)
        {
            Main.Randomizer.LogError("Loot.GiveLoot " + __instance.lootType);
            return false;
        }
    }
}