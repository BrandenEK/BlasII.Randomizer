using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle.Locks;

internal class LockPlacer : ILockPlacer
{
    public void Place(LocationPool locations, ItemPool items, out List<Lock> locks)
    {
        locks = new List<Lock>();
    }
}
