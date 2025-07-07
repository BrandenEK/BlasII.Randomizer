using System;

namespace BlasII.Randomizer.Shuffle.PoolCreators;

internal interface IItemPoolCreator
{
    public void Create(Random rng, RandomizerSettings settings, out ItemPool progression, out ItemPool junk);
}
