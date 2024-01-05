using BlasII.Randomizer.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.Randomizer.Items.Shuffle
{
    internal class ItemShufflerFunctional : IShuffler
    {
        private readonly IEnumerable<ItemLocation> _allLocations;
        private readonly IEnumerable<Item> _allItems;
        private readonly IDictionary<string, DoorLocation> _allDoors;
        private readonly Func<string, Item> _itemFinder;

        public ItemShufflerFunctional(IEnumerable<ItemLocation> locations, IEnumerable<Item> items, IDictionary<string, DoorLocation> doors, Func<string, Item> itemFinder)
        {
            _allLocations = locations;
            _allItems = items;
            _allDoors = doors;
            _itemFinder = itemFinder;
        }

        public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
        {
            output.Clear();

            // Create location and item pools
            var (progLocations, junkLocations) = _allLocations.Split(loc => loc.ShouldBeShuffled(settings));
            var (progItems, junkItems) = _allItems.AddVarious(item => item.count).Split(item => item.progression);

            progItems = DrainItemPool(progItems, GetStartingItems(settings));
            junkItems = BalanceItemPool(junkItems, progItems.Count() + junkItems.Count() - progLocations.Count() - junkLocations.Count());

            // Create initial inventory
            var inventory = CreateInitialInventory(settings, progItems, _allDoors);
            return true;
        }

        private IEnumerable<Item> DrainItemPool(IEnumerable<Item> progItems, IEnumerable<Item> drain) =>
            progItems.Where(item => !drain.Contains(item));

        private IEnumerable<Item> BalanceItemPool(IEnumerable<Item> junkItems, int balanceFactor) => balanceFactor switch
        {
            > 0 => junkItems.Concat(Enumerable.Repeat(_itemFinder("Tears[800]"), balanceFactor)),
            < 0 => junkItems.Take(junkItems.Count() + balanceFactor),
            _ => junkItems
        };

        private IEnumerable<Item> GetStartingItems(RandomizerSettings settings) =>
            new Item[] { _itemFinder(GetStartingWeaponId(settings)) };

        private Blas2Inventory CreateInitialInventory(RandomizerSettings settings, IEnumerable<Item> progItems, IDictionary<string, DoorLocation> allDoors)
        {
            // Add the starting weapon
            var inventory = new Blas2Inventory(settings, allDoors);
            inventory.AddItem(Main.Randomizer.Data.GetItem(GetStartingWeaponId(settings)));

            // Add all progression items in the pool
            foreach (var item in progItems)
            {
                inventory.AddItem(item);
            }
            return inventory;
        }

        private string GetStartingWeaponId(RandomizerSettings settings) => "WE0" + (settings.RealStartingWeapon + 1);
    }

        //internal class ItemShufflerReverse2 : IShuffler
        //{
        //    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
        //    {
        //        // ...

        //        // Place progression items at progression locations
        //        FillProgressionItems(progressionLocations, progressionItems, output, inventory);

        //        // Add junk items to remaining progression items and ensure they are still balanced
        //        junkLocations.AddRange(progressionLocations);
        //        if (junkLocations.Count != junkItems.Count)
        //            return false;

        //        // Place junk items at junk locations and remaining progression locations
        //        FillJunkItems(junkLocations, junkItems, output);

        //        // Verify that all remaining items were placed
        //        return junkItems.Count == 0 && junkLocations.Count == 0;
        //    }

        //    #region Shuffle

        //    /// <summary>
        //    /// Using a reverse fill algorithm, place the last item at a random reachable location
        //    /// </summary>
        //    private void FillProgressionItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output, Blas2Inventory inventory)
        //    {
        //        // Verify that all locations are reachable
        //        foreach (var location in locations)
        //        {
        //            if (!inventory.Evaluate(location.logic))
        //                return;
        //        }

        //        ShuffleList(items);
        //        MovePriorityItems(items);
        //        var reachableLocations = new List<ItemLocation>(locations);

        //        while (reachableLocations.Count > 0 && items.Count > 0)
        //        {
        //            Item item = RemoveLast(items);
        //            inventory.RemoveItem(item);

        //            RemoveUnreachableLocations(reachableLocations, inventory);
        //            if (reachableLocations.Count == 0)
        //                return;

        //            ItemLocation location = RemoveRandom(reachableLocations);
        //            locations.Remove(location);
        //            output.Add(location.id, item.id);
        //        }
        //    }

        //    /// <summary>
        //    /// Without logic, place the last item at a random location
        //    /// </summary>
        //    private void FillJunkItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        //    {
        //        ShuffleList(items);

        //        while (locations.Count > 0 && items.Count > 0)
        //        {
        //            ItemLocation location = RemoveLast(locations);
        //            Item item = RemoveLast(items);

        //            output.Add(location.id, item.id);
        //        }
        //    }

        //    /// <summary>
        //    /// After shuffling the list of progression items, move certain items to the end to prevent failing seeds
        //    /// </summary>        
        //    private void MovePriorityItems(List<Item> progressionItems)
        //    {
        //        Item wallClimb = Main.Randomizer.Data.GetItem("AB44");
        //        progressionItems.Remove(wallClimb);
        //        progressionItems.Insert(0, wallClimb);
        //    }

        //    /// <summary>
        //    /// Finds all locations that are no longer reachable and removes them
        //    /// </summary>
        //    private void RemoveUnreachableLocations(List<ItemLocation> locations, Blas2Inventory inventory)
        //    {
        //        for (int i = 0; i < locations.Count; i++)
        //        {
        //            if (inventory.Evaluate(locations[i].logic))
        //                continue;

        //            locations.RemoveAt(i);
        //            i--;
        //        }
        //    }

        //    #endregion

        //    // Old base shuffler stuff

        //    private Random rng;

        //    protected void Initialize(int seed) => rng = new Random(seed);

        //    protected int RandomInteger(int max) => rng.Next(max);

        //    protected T RandomElement<T>(List<T> list) => list[RandomInteger(list.Count)];

        //    protected void ShuffleList<T>(List<T> list)
        //    {
        //        int upperIdx = list.Count;
        //        while (upperIdx > 1)
        //        {
        //            upperIdx--;
        //            int randIdx = RandomInteger(upperIdx + 1);
        //            T value = list[randIdx];
        //            list[randIdx] = list[upperIdx];
        //            list[upperIdx] = value;
        //        }
        //    }

        //    protected T RemoveRandom<T>(List<T> list)
        //    {
        //        int index = RandomInteger(list.Count);
        //        T element = list[index];

        //        list.RemoveAt(index);
        //        return element;
        //    }

        //    protected T RemoveRandomFromOther<T>(List<T> list, List<T> other)
        //    {
        //        T element = RandomElement(list);

        //        other.Remove(element);
        //        return element;
        //    }

        //    protected T RemoveLast<T>(List<T> list)
        //    {
        //        int index = list.Count - 1;
        //        T element = list[index];

        //        list.RemoveAt(index);
        //        return element;
        //    }
        //}
}
