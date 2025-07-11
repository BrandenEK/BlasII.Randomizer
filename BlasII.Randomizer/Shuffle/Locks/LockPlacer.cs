using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System.Collections.Generic;

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
        locks =
        [
            //Z1064.i0
            new Lock(_locations["Z0105.l3"], _items["QI69"]), // Incense of the Envoys
            new Lock(_locations["Z2834.l0"], _items["PR101"]), // Prayer of the Penitent One

            //new Lock(_locations["Z0406.l4"], _items["AirJump"]), // test
        ];

        foreach (var lck in locks)
        {
            locations.Remove(lck.Location);
            items.Remove(lck.Item);
        }
    }
}
