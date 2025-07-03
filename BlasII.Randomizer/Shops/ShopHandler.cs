using BlasII.Randomizer.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer.Shops;

/// <summary>
/// Stores information for the shops
/// </summary>
public class ShopHandler
{
    /// <summary>
    /// Calculates the item costs for the specified shop
    /// </summary>
    public IEnumerable<int> GetShopCosts(string name, RandomizerSettings settings)
    {
        if (!_shops.TryGetValue(name, out IShop shop))
            throw new System.Exception($"Invalid shop id: {name}");

        if (settings.ShopMultiplier >= 4 || settings.ShopMultiplier < 0)
            return shop.GetVanillaCosts();

        var costs = new List<int>();

        foreach (var (vanillaCost, index) in shop.GetVanillaCosts().Select((value, i) => (value, i)))
        {
            string locationId = $"{name}.o{index}";
            Item item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);

            // If invalid item, place extreme cost
            if (!item.IsValid())
            {
                costs.Add(999999);
                continue;
            }

            // Based on item value, place random cost

            // Get ShopValue from item extensions
            // Get price range from shop handler
            float multiplier = settings.ShopMultiplier / 2f;
            Vector2 range = new Vector2(500, 1000) * multiplier / 10;

            int seed = (settings.Seed % 100000) * (shop.GetSeedValue() + index);
            int price = new System.Random(seed).Next((int)range.x, (int)range.y + 1);
            costs.Add(price * 10);
        }

        return costs;
    }

    public IShop TEMP_GetShop(string name)
    {
        return _shops[name];
    }

    private readonly Dictionary<ShopValue, Vector2> _priceRanges = new()
    {

    };

    private readonly Dictionary<string, IShop> _shops = new()
    {
        { "SHOPHAND", new HandShop() },
        { "SHOPMISSABLES", new PatioShop() },
        { "SHOPITINERANT", new FakeTravelerShop() },
    };
}
