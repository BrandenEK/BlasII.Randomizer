using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Locks;

internal class FakeLockPlacer : ILockPlacer
{
    public void Place(RandomizerSettings settings, LocationPool locations, ItemPool items, out List<Lock> locks)
    {
        locks = new List<Lock>();
    }
}
