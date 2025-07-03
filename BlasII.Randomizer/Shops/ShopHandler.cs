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
            float multiplier = settings.ShopMultiplier / 2f;
            Vector2 range = _priceRanges[item.GetValue()] * multiplier / 10;
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
        { ShopValue.FillerInventory, new Vector2(100, 200) },
        { ShopValue.UsefulInventory, new Vector2(1000, 2500) },
        { ShopValue.ProgressionInventory, new Vector2(2500, 3500) },
        { ShopValue.WeaponsAndAbilities, new Vector2(4000, 5000) },
        { ShopValue.Cherubs, new Vector2(400, 800) },
        { ShopValue.BossKeys, new Vector2(5500, 7500) },
        { ShopValue.LowTears, new Vector2(600, 2000) },
        { ShopValue.HighTears, new Vector2(1800, 5200) },
    };

    private readonly Dictionary<string, IShop> _shops = new()
    {
        { "SHOPHAND", new HandShop() },
        { "SHOPMISSABLES", new PatioShop() },
        { "SHOPITINERANT", new FakeTravelerShop() },
    };
}
