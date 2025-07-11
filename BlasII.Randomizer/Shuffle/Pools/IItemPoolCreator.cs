using BlasII.Randomizer.Shuffle.Models;
using System;

namespace BlasII.Randomizer.Shuffle.Pools;

internal interface IItemPoolCreator
{
    public void Create(Random rng, RandomizerSettings settings, out ItemPool progression, out ItemPool junk);
}
