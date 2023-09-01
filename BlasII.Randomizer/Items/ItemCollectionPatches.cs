using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.StatsSystem;
using Il2CppTGK.Game.Components.StatsSystem.Data;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.Loot;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            Main.Randomizer.LogError("Loot.GiveLoot");
            Main.Randomizer.LogError("Type: " + __instance.lootType.ToString());

            if (__instance.lootType != Loot.LootType.Item)
                return true;

            ItemID item = __instance.itemIdRef.LoadAsset().WaitForCompletion();
            string locationId = "ITEM_" + item.name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);


            //window.OnShow();

            //PopupMessageLogic message = Object.FindObjectOfType<PopupMessageLogic>();
            //Main.Randomizer.Log("Found message: " + (message != null));

            //message.ShowMessageAndWait(new Il2CppTGK.Game.PopupMessages.PopupMessage() { message = "Test" });

            //return false;

            //if (prevref == null)
            //{
            //    prevref = __instance.itemIdRef;
            //    Main.Randomizer.Log("Storing id asset");
            //}
            //else
            //{
            //    Main.Randomizer.AssetLoader.AddLoader(prevref.LoadAsset(), CheckLocation);
            //    Main.Randomizer.AssetLoader.AddLoader(__instance.itemIdRef.LoadAsset(), CheckLocation);
            //}

            //AsyncOperationHandle<ItemID> handle = __instance.itemIdRef.LoadAsset();
            //ItemID item = handle.WaitForCompletion();
            //Main.Randomizer.LogWarning("Item: " + item.name);

            //Main.Randomizer.AssetLoader.AddLoader(__instance.itemIdRef.LoadAsset(), CheckLocation);
            //Main.Randomizer.AssetLoader.AddLoader(__instance.itemIdRef.LoadAsset(), CheckLocation);


            return true;
        }

        private static ItemIDAssetReference prevref;

        private static void CheckLocation(ItemID item)
        {
            string locationId = "ITEM_" + item.name;
            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
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

            //ItemPopupWindowLogic window = Object.FindObjectOfType<ItemPopupWindowLogic>(true);
            //if (window == null)
            //{
            //    Main.Randomizer.LogError("Failed to find window");
            //    return true;
            //}
                    ItemID item = __instance.itemID;

            //ItemPopupWindowLogic window = CoreCache.UINavigationHelper.itemPopupWindowLogic;
            //Main.Randomizer.LogWarning("Window exists: " + (window != null));

            //window.ShowPopup("Found item: ", item.caption, item.image);

            //__instance.showPopup = false;
            __instance.Finish();

            CoreCache.UINavigationHelper.ShowItemPopup("Found item: ", item.caption, item.image);
            //foreach (UIWindowLogic window in CoreCache.UIManager.orderedWindows)
            //{
            //    Main.Randomizer.LogWarning("Window: " + window.name);
            //    if (window is ItemPopupWindowLogic itemWindow)
            //    {
            //        itemWindow.ShowPopup("Found item: ", item.caption, item.image);
            //    }
            //}


            return false;
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