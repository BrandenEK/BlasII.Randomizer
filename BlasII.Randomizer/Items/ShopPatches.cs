using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.ShopSystem;
using Il2CppTGK.Inventory;
using UnityEngine;

namespace BlasII.Randomizer.Items
{
    [HarmonyPatch(typeof(Shop), nameof(Shop.CacheData))]
    class Shop_Cache_Path
    {
        public static void Postfix(Shop __instance)
        {
            foreach (var item in __instance.cachedShopDataById)
            {
                Main.Randomizer.LogWarning($"{item.Key}: {item.Value.itemID.name} for {item.Value.price}");
            }

            Main.Randomizer.Log("Caching shop: " + __instance.name);

            //var censer = ScriptableObject.CreateInstance<ItemID>();
            //censer.name = "WE01";
            //censer.caption = "Veredicto";
            //censer.description = "It is a censer";
            //censer.lore = "Does it really need lore";
            //censer.image = Main.Randomizer.Data.GetImage(DataStorage.ImageType.Censer);

            //int currentIdx = 0, totalItems = __instance.cachedIds.Count;
            //foreach (var shopitem in __instance.cachedShopDataById.Values)
            //{
            //    var reward = ItemStorage.TryGetQuestItem("QI" + (currentIdx + 1).ToString("00"), out var lance) ? lance : null;
            //    __instance.cachedIds[currentIdx] = reward;
            //    shopitem.itemID = reward;
            //    currentIdx++;
            //}

            //for (int i = 0; i < __instance.cachedIds.Count; i++)
            //{
            //    Main.Randomizer.LogError(__instance.cachedIds[i].name + " to lance");
            //    __instance.cachedIds[i] = ItemStorage.TryGetQuestItem("QI70", out var lance) ? lance : null;
            //}

            //foreach (int id in __instance.cachedShopDataById.Keys)
            //{
            //    Main.Randomizer.LogError(__instance.cachedShopDataById[id].itemID.name + " to lance");
            //    __instance.cachedShopDataById[id].itemID = ItemStorage.TryGetQuestItem("QI70", out var lance) ? lance : null;
            //}

            //foreach (var item in __instance.cachedShopDataByType[Shop.ItemType.All].commonElements)
            //{
            //    Main.Randomizer.LogError(item.itemID.name);
            //}

            __instance.cachedIds.Clear();
            __instance.cachedShopDataById.Clear();
            __instance.cachedShopDataByType.Clear();

            //__instance.cachedShopDataByType.Remove(Shop.ItemType.QuestItem);
            //__instance.cachedShopDataByType.Remove(Shop.ItemType.Prayers);
            //__instance.cachedShopDataByType.Remove(Shop.ItemType.RosaryBead);
            //__instance.cachedShopDataByType.Remove(Shop.ItemType.Figures);

            __instance.orbs.Clear();
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

    [HarmonyPatch(typeof(ShopManager), nameof(ShopManager.SellOrb))]
    class Shop_Sell_Patch
    {
        public static bool Prefix(Shop shop, int orbIdx)
        {
            Main.Randomizer.LogWarning("Buying item in shop: " +  shop.name + "." + orbIdx);
            Main.Randomizer.ItemHandler.GiveItemAtLocation("Z1501.s0");
            return false;
        }
    }
}
