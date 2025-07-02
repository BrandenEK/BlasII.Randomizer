using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shops;

/// <summary>
/// Stores information for the shops
/// </summary>
public class ShopHandler
{
    /// <summary>
    /// Calculates the item costs for the specified shop
    /// </summary>
    public IEnumerable<int> GetShopCosts(string name)
    {
        if (!_shops.TryGetValue(name, out IShop shop))
            throw new Exception($"Invalid shop id: {name}");

        return shop.GetVanillaCosts();
    }

    public IShop TEMP_GetShop(string name)
    {
        return _shops[name];
    }

    private readonly Dictionary<string, IShop> _shops = new()
    {
        { "SHOPHAND", new HandShop() },
        { "SHOPMISSABLES", new PatioShop() },
        { "SHOPITINERANT", new FakeTravelerShop() },
    };
}
