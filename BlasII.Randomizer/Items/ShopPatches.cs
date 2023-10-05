using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.UI;
using UnityEngine;
using UnityEngine.UI;

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

            // Clear all previous items from list
            __instance.cachedIds.Clear();
            __instance.cachedShopDataById.Clear();
            __instance.cachedShopDataByType.Clear();
            __instance.orbs.Clear();

            // Get list of costs based on shop id
            int[] costs = __instance.name switch
            {
                "SHOPHAND" => new int[] { 3000, 3000, 3000, 3000, 3000, 3000, 3000, 6000, 12000, 12000, 17500, 32000 },
                "SHOPITINERANT" => new int[] { 3000, 3000, 6000, 6000, 17500, 12000, 6000, 6000 },
                "SHOPMISSABLES" => new int[] { 6000, 6000, 12000, 12000, 12000, 6000, 6000 },
                _ => System.Array.Empty<int>()
            };

            // Add orbs for each price
            foreach (int cost in costs)
                __instance.orbs.Add(cost);
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

            var item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);
            __instance.Caption = item.Upgraded.name;
            __instance.Description = item.Upgraded.Description;
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

            var item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);
            data.obj.transform.Find("Image").GetComponent<Image>().sprite = item.Upgraded.Image;
        }
    }
}
