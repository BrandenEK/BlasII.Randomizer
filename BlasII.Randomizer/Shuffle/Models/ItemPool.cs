using BlasII.Randomizer.Models;
using System;

namespace BlasII.Randomizer.Shuffle.Models;

internal class ItemPool : BasePool<Item>
{
    public ItemPool(Random rng) : base(rng) { }

    public ItemPool(ItemPool pool) : base(pool) { }
}
