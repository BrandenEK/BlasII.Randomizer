using BlasII.Randomizer.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Pools;

internal class PoolBalancer : IPoolBalancer
{
    private readonly Dictionary<string, Item> _items;

    public PoolBalancer(Dictionary<string, Item> items)
    {
        _items = items;
    }

    public void Balance(LocationPool progLocations, LocationPool junkLocations, ItemPool progItems, ItemPool junkItems)
    {
        int difference = (progItems.Size + junkItems.Size) - (progLocations.Size + junkLocations.Size);

        if (difference > 0)
        {
            RemoveTearItems(junkItems, difference);
        }
        else if (difference < 0)
        {
            AddTearItems(junkItems, -difference);
        }
    }

    private void RemoveTearItems(ItemPool pool, int amount)
    {
        for (int i = 0; i < amount; i++)
            pool.RemoveLast();
    }

    private void AddTearItems(ItemPool pool, int amount)
    {
        pool.Add(_items["Tears[800]"], amount);
    }
}
