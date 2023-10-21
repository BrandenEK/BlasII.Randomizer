using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    internal class ItemShufflerReverse : BaseShuffler
    {
        public override bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
        {
            output.Clear();
            Initialize(seed);

            // Create pools of all locations to randomize
            List<ItemLocation> progressionLocations = new(), junkLocations = new();
            CreateLocationPool(progressionLocations, junkLocations, settings);

            // Create pools of all items to randomize
            List<Item> progressionItems = new(), junkItems = new();
            CreateItemPool(progressionItems, junkItems, progressionLocations.Count + junkLocations.Count, settings);

            // Create initial inventory
            var inventory = new Blas2Inventory(settings, Main.Randomizer.Data.DoorDictionary);
            CreateInitialInventory(inventory, settings, progressionItems);

            // Place progression items at progression locations
            FillProgressionItems(progressionLocations, progressionItems, output, inventory);

            // Verify that all progression items were placed
            if (progressionItems.Count > 0)
                return false;

            // Place junk items at junk locations and remaining progression locations
            junkLocations.AddRange(progressionLocations);
            FillJunkItems(junkLocations, junkItems, output);

            // Verify that all remaining items were placed
            return junkItems.Count == 0;
        }

        /// <summary>
        /// Fills the two location pools
        /// </summary>
        private void CreateLocationPool(List<ItemLocation> progressionLocations, List<ItemLocation> junkLocations, RandomizerSettings settings)
        {
            foreach (var location in Main.Randomizer.Data.ItemLocationList)
            {
                AddLocationToPool(progressionLocations, junkLocations, location, settings);
            }
        }

        /// <summary>
        /// Takes a single location data and adds it to the correct list based on its type
        /// </summary>
        private void AddLocationToPool(List<ItemLocation> progressionLocations, List<ItemLocation> junkLocations, ItemLocation location, RandomizerSettings settings)
        {
            List<ItemLocation> locationPool = location.ShouldBeShuffled(settings) ? progressionLocations : junkLocations;
            locationPool.Add(location);
        }

        /// <summary>
        /// Fills the two item pools to match the number of locations
        /// </summary>
        private void CreateItemPool(List<Item> progressionItems, List<Item> junkItems, int numOfLocations, RandomizerSettings settings)
        {
            foreach (var item in Main.Randomizer.Data.ItemList)
            {
                AddItemToPool(progressionItems, junkItems, item);
            }

            RemoveStartingItemsFromItemPool(progressionItems, settings);
            BalanceItemPool(progressionItems, junkItems, numOfLocations);
        }

        /// <summary>
        /// Takes a single item data and adds it to the correct list based on its type
        /// </summary>
        private void AddItemToPool(List<Item> progressionItems, List<Item> junkItems, Item item)
        {
            List<Item> itemPool = item.progression ? progressionItems : junkItems;
            for (int i = 0; i < item.count; i++)
            {
                itemPool.Add(item);
            }
        }

        /// <summary>
        /// Removes the starting weapon from the item pool
        /// </summary>
        private void RemoveStartingItemsFromItemPool(List<Item> items, RandomizerSettings settings)
        {
            // Remove the extra starting weapon
            items.Remove(Main.Randomizer.Data.GetItem(GetStartingWeaponId(settings)));
        }

        /// <summary>
        /// Adds the starting weapon and all progression items to the initial inventory
        /// </summary>
        private void CreateInitialInventory(Blas2Inventory inventory, RandomizerSettings settings, List<Item> progressionItems)
        {
            // Add the starting weapon
            inventory.AddItem(Main.Randomizer.Data.GetItem(GetStartingWeaponId(settings)));

            // Add all progression items in the pool
            foreach (var item in progressionItems)
            {
                inventory.AddItem(item);
            }
        }

        /// <summary>
        /// Calculates the item id of the chosen starting weapon
        /// </summary>
        private string GetStartingWeaponId(RandomizerSettings settings)
        {
            return "WE0" + (settings.RealStartingWeapon + 1);
        }

        /// <summary>
        /// After creating the item pool, add or remove junk items to make it equal to the number of locations
        /// </summary>
        private void BalanceItemPool(List<Item> progressionItems, List<Item> junkItems, int numOfLocations)
        {
            // Remove tear items until pools are equal
            while (progressionItems.Count + junkItems.Count > numOfLocations)
            {
                junkItems.RemoveAt(junkItems.Count - 1);
            }

            // Add tear items until pools are equal
            while (progressionItems.Count + junkItems.Count < numOfLocations)
            {
                junkItems.Add(Main.Randomizer.Data.GetItem("Tears[800]"));
            }
        }

        /// <summary>
        /// Using a reverse fill algorithm, place the last item at a random reachable location
        /// </summary>
        private void FillProgressionItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output, Blas2Inventory inventory)
        {
            // Verify that all locations are reachable
            foreach (var location in locations)
            {
                if (!inventory.Evaluate(location.logic))
                    return;
            }

            ShuffleList(items);
            MovePriorityItems(items);
            var reachableLocations = new List<ItemLocation>(locations);

            while (reachableLocations.Count > 0 && items.Count > 0)
            {
                Item item = RemoveLast(items);
                inventory.RemoveItem(item);

                RemoveUnreachableLocations(reachableLocations, inventory);
                if (reachableLocations.Count == 0)
                    return;

                ItemLocation location = RemoveRandom(reachableLocations);
                locations.Remove(location);
                output.Add(location.id, item.id);
            }
        }

        /// <summary>
        /// After shuffling the list of progression items, move certain items to the end to prevent failing seeds
        /// </summary>        
        private void MovePriorityItems(List<Item> progressionItems)
        {
            Item wallClimb = Main.Randomizer.Data.GetItem("AB44");
            progressionItems.Remove(wallClimb);
            progressionItems.Insert(0, wallClimb);
        }

        /// <summary>
        /// Finds all locations that are no longer reachable and removes them
        /// </summary>
        private void RemoveUnreachableLocations(List<ItemLocation> locations, Blas2Inventory inventory)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                if (inventory.Evaluate(locations[i].logic))
                    continue;

                locations.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Without logic, place the last item at a random location
        /// </summary>
        private void FillJunkItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        {
            ShuffleList(items);

            while (locations.Count > 0 && items.Count > 0)
            {
                ItemLocation location = RemoveLast(locations);
                Item item = RemoveLast(items);

                output.Add(location.id, item.id);
            }
        }
    }
}
