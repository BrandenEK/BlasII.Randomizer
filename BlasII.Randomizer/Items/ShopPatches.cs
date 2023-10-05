using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.Inventory;
using Il2CppTGK.UI;
using UnityEngine;
using UnityEngine.UI;
using static MelonLoader.MelonLogger;

namespace BlasII.Randomizer.Items
{
    [HarmonyPatch(typeof(Shop), nameof(Shop.CacheData))]
    class Shop_Cache_Path
    {
        public static void Postfix(Shop __instance)
        {
            // Log temp info
            Main.Randomizer.Log("Caching shop: " + __instance.name);
            foreach (var item in __instance.cachedShopDataById)
            {
                Main.Randomizer.LogWarning($"{item.Key}: {item.Value.itemID.name} for {item.Value.price}");
            }
            // SHOPHAND
            // SHOPITINERANT

            // Clear all previous items from list
            __instance.cachedIds.Clear();
            __instance.cachedShopDataById.Clear();
            __instance.cachedShopDataByType.Clear();
            __instance.orbs.Clear();

            // Add orbs for each price
            __instance.orbs.Add(50);
            __instance.orbs.Add(50);
            __instance.orbs.Add(50);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
        }
    }

    /// <summary>
    /// When purchasing an "orb" instead give the random item
    /// </summary>
    [HarmonyPatch(typeof(ShopManager), nameof(ShopManager.SellOrb))]
    class Shop_Sell_Patch
    {
        public static void Postfix(Shop shop, int orbIdx)
        {            
            string locationId = $"{shop.name}.o{orbIdx}";
            Main.Randomizer.LogError("ShopManager.SellOrb - " + locationId);

            Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
        }
    }

    /// <summary>
    /// When opening the shop or purchasing an item, update this item's name and description
    /// </summary>
    [HarmonyPatch(typeof(ShopListItem), nameof(ShopListItem.SetData))]
    class Shop_Text_Patch
    {
        public static void Postfix(ShopListItem __instance)
        {
            string locationId = $"{Object.FindObjectOfType<ShopWindowLogic>().currentShop.name}.o{__instance.OrbIdx}";
            Main.Randomizer.LogWarning("Setting data for " + locationId);
            var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            __instance.Caption = lance.Upgraded.name;
            __instance.Description = lance.Description;
        }
    }

    /// <summary>
    /// When opening the shop or purchasing an item, update this item's image
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.OnCreateListItem))]
    class Shop_Image_Patch
    {
        public static void Postfix(UINavigableScrollableList.ScrollableListData data)
        {
            string locationId = $"{Object.FindObjectOfType<ShopWindowLogic>().currentShop.name}.o{data.obj.GetComponent<ShopListItem>().OrbIdx}";
            Main.Randomizer.LogWarning("Setting image for " + locationId);
            var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            data.obj.transform.Find("Image").GetComponent<Image>().sprite = lance.Upgraded.Image;
        }
    }
}
