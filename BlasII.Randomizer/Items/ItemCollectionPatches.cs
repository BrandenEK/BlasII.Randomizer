using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
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
            Main.Randomizer.LogError("LootInteractable.UseLootByInteractor");

            string locationId = $"l{__instance.transform.GetSiblingIndex()}.{CoreCache.Room.CurrentRoom.Name}";
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

            //__instance.loot = null;
        }

    }

    // When an item if given through playermaker, such as dialog
    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class PlayerMaker_AddItem_Patch
    {
        public static bool Prefix(AddItem __instance)
        {
            Main.Randomizer.LogError("AddItem.OnEnter");

            string locationId = $"i{__instance.owner.transform.GetSiblingIndex()}.{CoreCache.Room.CurrentRoom.Name}";
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

            //__instance.Finish();
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

            string locationId = $"w0.{CoreCache.Room.CurrentRoom.Name}";
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

            //__instance.Finish();
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

            string locationId = $"a0.{CoreCache.Room.CurrentRoom.Name}";
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

            //__instance.Finish();
            return true;
        }
    }
}