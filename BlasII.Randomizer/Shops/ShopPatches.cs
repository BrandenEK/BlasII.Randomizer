using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Extensions;
using BlasII.Randomizer.Models;
using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Shops;

/// <summary>
/// Force order recalculation of shop items
/// </summary>
[HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.UpdateTabs))]
class ShopWindowLogic_UpdateTabs_Patch
{
    public static void Postfix(ShopWindowLogic __instance)
    {
        ModLog.Info($"ShopWindowLogic.UpdateTabs ({__instance.currentShop.name})");

        // Get list of costs based on shop id
        var costs = Main.Randomizer.ShopHandler.GetShopCosts(__instance.currentShop.name, Main.Randomizer.CurrentSettings);

        // Clear all cached values
        __instance.currentShop.cachedIds.Clear();
        __instance.currentShop.cachedShopDataById.Clear();
        __instance.currentShop.cachedShopDataByType.Clear();
        __instance.currentShop.orbs.Clear();

        // Recalculate the orbs
        foreach (int cost in costs)
            __instance.currentShop.orbs.Add(cost);

        // Recalculate the cachedElements
        if (__instance.cachedElements.ContainsKey(Shop.ItemType.All))
        {
            var realList = __instance.cachedElements[Shop.ItemType.All];
            var tempList = new List<Shop.CachedShopDataItem>();

            foreach (var item in realList)
                tempList.Add(item);

            realList.Clear();

            foreach (var item in tempList.OrderBy(x => x.orbIdx))
                realList.Add(item);
        }
    }
}

/// <summary>
/// Force order recalculation of shop items
/// </summary>
[HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.ShowShop))]
class ShopWindowLogic_ShowShop_Patch
{
    public static void Prefix(Shop shop, ShopWindowLogic __instance)
    {
        ModLog.Info($"ShopWindowLogic.ShowShop ({shop.name})");

        // Get list of costs based on shop id
        var costs = Main.Randomizer.ShopHandler.GetShopCosts(shop.name, Main.Randomizer.CurrentSettings);

        // Clear all cached values
        shop.cachedIds.Clear();
        shop.cachedShopDataById.Clear();
        shop.cachedShopDataByType.Clear();
        shop.orbs.Clear();

        // Recalculate the orbs
        foreach (int cost in costs)
            __instance.currentShop.orbs.Add(cost);

        // Recalculate the cachedElements
        if (__instance.cachedElements.ContainsKey(Shop.ItemType.All))
        {
            var realList = __instance.cachedElements[Shop.ItemType.All];
            var tempList = new List<Shop.CachedShopDataItem>();

            foreach (var item in realList)
                tempList.Add(item);

            realList.Clear();

            foreach (var item in tempList.OrderBy(x => x.orbIdx))
                realList.Add(item);
        }
    }
}

/// <summary>
/// When purchasing an "orb" instead give the random item
/// </summary>
[HarmonyPatch(typeof(ShopManager), nameof(ShopManager.SellOrb))]
class ShopManager_SellOrb_Patch
{
    public static void Postfix(Shop shop, int orbIdx)
    {
        string locationId = shop.CalculateId(orbIdx);
        ModLog.Custom("ShopManager.SellOrb - " + locationId, System.Drawing.Color.Green);

        Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);

        // When selling an "orb", it still displays and give an orb.  So take one away from the player, and patch the display method
        AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["Orbs"], -1);
    }
}

/// <summary>
/// When opening the shop or purchasing an item, update this item's name and description
/// </summary>
[HarmonyPatch(typeof(ShopListItem), nameof(ShopListItem.SetData))]
class ShopListItem_SetData_Patch
{
    public static void Postfix(ShopListItem __instance)
    {
        string locationId = $"{Object.FindObjectOfType<ShopWindowLogic>().currentShop.name}.o{__instance.OrbIdx}";

        var item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);
        __instance.Caption = item.GetName();
        __instance.Description = item.GetDescription();
    }
}

/// <summary>
/// When opening the shop or purchasing an item, update this item's image
/// </summary>
[HarmonyPatch(typeof(ShopWindowLogic), nameof(ShopWindowLogic.OnCreateListItem))]
class ShopWindowLogic_OnCreateListItem_Patch
{
    public static void Postfix(UINavigableScrollableList.ScrollableListData data)
    {
        string locationId = $"{Object.FindObjectOfType<ShopWindowLogic>().currentShop.name}.o{data.obj.GetComponent<ShopListItem>().OrbIdx}";

        var item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);
        var sprite = item.GetSprite();

        Image image = data.obj.transform.Find("Image").GetComponent<Image>();
        Vector2 size = sprite == null ? new Vector2(90, 90) : sprite.rect.size * 3;
        Vector2 offset = new((90 - size.x) / 2, 0);

        image.sprite = sprite;
        image.rectTransform.sizeDelta = size;
        image.rectTransform.anchoredPosition += offset;
    }
}

/// <summary>
/// When displaying an orb item, instantly hide it
/// </summary>
[HarmonyPatch(typeof(OrbsRewardPopupLogic), nameof(OrbsRewardPopupLogic.ShowPopup))]
class OrbsRewardPopupLogic_ShowPopup_Patch
{
    public static void Prefix(OrbsRewardPopupLogic __instance)
    {
        __instance.timeToShowMessage = 0;
        __instance.fadeInTime = 0;
        __instance.fadeOutTime = 0;
    }
}
