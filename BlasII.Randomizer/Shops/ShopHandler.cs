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
        { "SHOPITINERANT", new TravelerShop() },
    };
}

//public interface IShop
//{
//    public IEnumerable<int> GetVanillaCosts();
//}

//public class StandardShop(int[] costs) : IShop
//{
//    public IEnumerable<int> GetVanillaCosts()
//    {
//        return costs;
//    }
//}

//public class ConditionalShop(ConditionalCost[] costs) : IShop
//{
//    public IEnumerable<int> GetVanillaCosts()
//    {
//        foreach (var cost in costs)
//        {
//            if (cost.Condition())
//                yield return cost.Cost;
//        }
//    }
//}

//public class ConditionalCost(int cost, Func<bool> condition)
//{
//    public int Cost { get; } = cost;

//    public Func<bool> Condition { get; } = condition;
//}

//private readonly Dictionary<string, IShop> _shops = new()
//{
//    { "SHOPHAND", new StandardShop([3000, 3000, 3000, 3000, 3000, 3000, 3000, 6000, 12000, 12000, 17500, 32000]) },
//    { "SHOPMISSABLES", new StandardShop([6000, 6000, 6000, 6000, 12000, 12000, 12000]) },
//    { "SHOPITINERANT", new ConditionalShop([
//        new ConditionalCost(3000, () => true),
//        new ConditionalCost(3000, () => true),
//        new ConditionalCost(6000, () => true),
//        new ConditionalCost(6000, () => true),
//        new ConditionalCost(6000, () => true),
//        new ConditionalCost(6000, () => false),
//        new ConditionalCost(12000, () => false),
//        new ConditionalCost(17500, () => false),
//    ]) },
//};