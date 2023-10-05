using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.Inventory;

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

            // Add orbs for each price
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(3000);
            __instance.orbs.Add(50);
            __instance.orbs.Add(50);
            __instance.orbs.Add(50);
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
    /// Whenever opening the shop or purchasing an item, update the icons for all items in the shop
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.UpdateTabs))]
    class Shop_Update_Patch
    {
        public static void Postfix(ShopWindowLogic __instance)
        {
            Main.Randomizer.LogWarning("Updating tabs!");
            Main.Randomizer.Log(__instance.transform.GetChild(0).GetChild(0).DisplayHierarchy(7, true));
        }
    }

    /// <summary>
    /// Whenever selecting a different item, update the name and description text
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.OnSelectIem))]
    class Shop_Select_Patch
    {
        public static void Postfix(ShopWindowLogic __instance)
        {
            Main.Randomizer.LogWarning("Selecting new item!");
            var lance = Main.Randomizer.Data.GetItem("QI70");

            __instance.captionText.SetText(lance.Upgraded.name);
            __instance.descriptionText.textControl.SetText(lance.Upgraded.Description);
        }
    }
}
