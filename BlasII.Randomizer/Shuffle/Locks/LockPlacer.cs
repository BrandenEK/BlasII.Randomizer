using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle.Models;
using System;
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
        locks = new List<Lock>();

        Console.WriteLine("locations: " + locations.Size);
        Console.WriteLine("items: " + items.Size);

        // Add global locks
        locks.Add(new Lock(_locations["Z1064.i0"], _items["QI69"])); // Incense of the Envoys
        locks.Add(new Lock(_locations["Z2834.l0"], _items["PR101"])); // Prayer of the Penitent One

        foreach (var lck in locks)
        {
            locations.Remove(lck.Location);
            items.Remove(lck.Item);
        }

        Console.WriteLine("locations: " + locations.Size);
        Console.WriteLine("items: " + items.Size);
    }
}
