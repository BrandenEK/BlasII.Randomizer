using System;

namespace BlasII.Randomizer.Shuffle.PoolCreators;

internal interface ILocationPoolCreator
{
    public void Create(Random rng, RandomizerSettings settings, out LocationPool progression, out LocationPool junk);
}
