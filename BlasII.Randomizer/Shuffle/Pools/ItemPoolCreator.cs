using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using BlasII.Randomizer.Storages;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Pools;

internal class ItemPoolCreator : IItemPoolCreator
{
    private readonly Dictionary<string, Item> _items;

    public ItemPoolCreator(Dictionary<string, Item> items)
    {
        _items = items;
    }

    public void Create(Random rng, RandomizerSettings settings, out ItemPool progression, out ItemPool junk)
    {
        progression = new ItemPool(rng);
        junk = new ItemPool(rng);

        if (settings.AddPenitenceRewards)
            AddPenitenceItemsToPool(junk);

        foreach (var item in _items.Values)
        {
            ItemPool pool = item.Class == Item.ItemClass.Progression ? progression : junk;
            pool.Add(item, item.Count);
        }

        RemoveStartingItemsFromPool(progression, settings);
    }

    private void AddPenitenceItemsToPool(ItemPool junk)
    {
        foreach (string id in ItemGroups.ChallengeRewards)
            junk.Add(_items[id]);
    }

    private void RemoveStartingItemsFromPool(ItemPool items, RandomizerSettings settings)
    {
        // Remove the extra starting weapon
        string weaponId = ((WEAPON_IDS)settings.RealStartingWeapon).ToString();
        items.Remove(_items[weaponId]);
    }
}
