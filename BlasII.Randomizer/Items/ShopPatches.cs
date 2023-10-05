using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.Inventory;
using Il2CppTGK.UI;
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
    /// When opening the shop or purchasing an item, update the icons for all items in the shop - not anymore
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.UpdateTabs))]
    class Shop_Update_Patch
    {
        public static void Postfix(ShopWindowLogic __instance)
        {
            Main.Randomizer.LogWarning("Updating tabs!");
        }
    }

    /// <summary>
    /// When selecting a different item, update the name and description text
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.OnSelectIem))]
    class Shop_Select_Patch
    {
        public static void Postfix(ShopWindowLogic __instance)
        {
            //Main.Randomizer.LogWarning("Selecting new item!");
            //var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            //__instance.captionText.SetText(lance.Upgraded.name);
            //__instance.descriptionText.textControl.SetText(lance.Upgraded.Description);
        }
    }

    /// <summary>
    /// When opening the confirmation box, change the item name displayed
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.ShowConfirmation))]
    class Shop_Confirm_Patch
    {
        public static void Postfix(ShopWindowLogic __instance)
        {
            //Main.Randomizer.LogWarning("Opening confirm box!");
            //var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            //string originalMessage = __instance.confirmationText.normalText.text;
            //string firstPart = originalMessage[..(originalMessage.IndexOf('>') + 1)];
            //string lastPart = originalMessage[originalMessage.LastIndexOf('<')..];
            //string newMessage = firstPart + lance.Upgraded.name + lastPart;

            //__instance.confirmationText.SetText(newMessage);
        }
    }

    /// <summary>
    /// When opening the shop or purchasing an item, update the icons for all items in the shop 
    /// </summary>
    [HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.OnCreateListItem))]
    class Shop_Move_Patch
    {
        public static void Postfix(UINavigableScrollableList.ScrollableListData data)
        {
            Main.Randomizer.LogWarning("Creating new scroll item!");
            var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            data.obj.transform.Find("Image").GetComponent<Image>().sprite = lance.Upgraded.Image;

            Main.Randomizer.Log(data.obj.name);
        }
    }

    /// <summary>
    /// Change the name and description for this shop item to its random item
    /// </summary>
    [HarmonyPatch(typeof(ShopListItem), nameof(ShopListItem.SetData))]
    class Shop_Data_Patch
    {
        public static void Postfix(ShopListItem __instance)
        {
            Main.Randomizer.LogWarning("Updating list item data!");
            var lance = Main.Randomizer.Data.GetItem("QI70"); // Get random item

            __instance.Caption = lance.Upgraded.name;
            __instance.Description = lance.Description;
        }
    }
}
