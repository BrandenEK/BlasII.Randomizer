using BlasII.Randomizer.Models;
using System;

namespace BlasII.Randomizer.Shuffle.Models;

internal class LocationPool : BasePool<ItemLocation>
{
    public LocationPool(Random rng) : base(rng) { }

    public LocationPool(LocationPool pool) : base(pool) { }
}
