using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Models;
using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Setup the randomized items for a shop
/// </summary>
[HarmonyPatch(typeof(Shop), nameof(Shop.CacheData))]
class Shop_CacheData_Patch
{
    public static void Postfix(Shop __instance)
    {
        // Clear all previous items from list
        __instance.cachedIds.Clear();
        __instance.cachedShopDataById.Clear();
        __instance.cachedShopDataByType.Clear();
        __instance.orbs.Clear();

        // Get list of costs based on shop id
        int[] costs;

        switch (__instance.name)
        {
            // Always same items, sorted by cost
            case "SHOPHAND":
                costs = new int[]
                {
                    3000, 3000, 3000, 3000, 3000, 3000, 3000, 6000, 12000, 12000, 17500, 32000
                };
                break;
            // Always same items, sorted by cost
            case "SHOPMISSABLES":
                costs = new int[]
                {
                    6000, 6000, 6000, 6000, 12000, 12000, 12000
                };
                break;
            // More items added for each location, sorted by cost
            case "SHOPITINERANT":
                var list = new List<int>
                {
                    3000, 3000
                };

                if (Main.Randomizer.GetQuestBool("ST06", "Z09_VISITED"))
                    list.Add(6000);
                if (Main.Randomizer.GetQuestBool("ST06", "Z05_VISITED"))
                    list.Add(6000);
                if (Main.Randomizer.GetQuestBool("ST06", "Z11_VISITED"))
                    list.Add(6000);
                if (Main.Randomizer.GetQuestBool("ST06", "Z12_VISITED"))
                    list.Add(6000);
                if (Main.Randomizer.GetQuestBool("ST06", "Z01_VISITED"))
                    list.Add(12000);
                if (Main.Randomizer.GetQuestBool("ST06", "Z10_VISITED"))
                    list.Add(17500);

                costs = list.ToArray();
                break;

            default:
                ModLog.Error("Opening invalid shop!");
                costs = System.Array.Empty<int>();
                break;
        }

        // Add orbs for each price
        foreach (int cost in costs)
            __instance.orbs.Add(cost);
        ModLog.Info("Updating items for: " + __instance.name);
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
        string locationId = $"{shop.name}.s{orbIdx}";
        ModLog.Error("ShopManager.SellOrb - " + locationId);

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
        Vector2 size = sprite.rect.size * 3;
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
