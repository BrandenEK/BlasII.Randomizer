using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Shuffle.Locks;

internal class LockPlacer : ILockPlacer
{
    private readonly Dictionary<string, ItemLocation> _locations;
    private readonly Dictionary<string, Item> _items;

    public LockPlacer(Dictionary<string, ItemLocation> locations, Dictionary<string, Item> items)
    {
        _locations = locations;
        _items = items;
    }

    public void Place(RandomizerSettings settings, LocationPool locations, ItemPool items, out List<Lock> locks)
    {
        // Only works if the item is marked as progression!
        locks = new List<Lock>();

        PlaceDefaultLocks(locks);

        if (!settings.ShuffleCherubs)
            PlaceCherubLocks(locks);

        foreach (var lck in locks)
        {
            locations.Remove(lck.Location);
            items.Remove(lck.Item);
        }
    }

    private void PlaceDefaultLocks(List<Lock> locks)
    {
        locks.Add(new Lock(_locations["Z1064.i0"], _items["QI69"], Lock.LockType.AddToBoth)); // Incense of the Envoys
        locks.Add(new Lock(_locations["Z2834.l0"], _items["PR101"], Lock.LockType.AddToBoth)); // Prayer of the Penitent One
    }

    private void PlaceCherubLocks(List<Lock> locks)
    {
        Item cherub = _items["CH"];
        foreach (ItemLocation location in _locations.Values.Where(x => x.HasFlag('C')))
            locks.Add(new Lock(location, cherub, Lock.LockType.AddToInventory));
    }
}
