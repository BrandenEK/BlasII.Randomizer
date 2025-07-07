using BlasII.Randomizer.Models;
using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.PoolCreators;

internal class LocationPoolCreator : ILocationPoolCreator
{
    private readonly IEnumerable<ItemLocation> _locations;

    public LocationPoolCreator(IEnumerable<ItemLocation> locations)
    {
        _locations = locations;
    }

    public void Create(Random rng, RandomizerSettings settings, out LocationPool progression, out LocationPool junk)
    {
        progression = new LocationPool(rng);
        junk = new LocationPool(rng);

        foreach (var location in _locations)
        {
            LocationPool pool = location.ShouldBeShuffled(settings) ? progression : junk;
            pool.Add(location);
        }
    }
}
