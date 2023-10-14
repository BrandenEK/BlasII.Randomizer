using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    internal class ItemShuffler : BaseShuffler
    {
        public override bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output)
        {
            output.Clear();
            Initialize(seed);

            // Create inventory with starting items
            var inventory = new Blas2Inventory();
            AddStartingItemsToInventory(inventory, settings);

            // Create pools of all locations to randomize
            List<ItemLocation> progressionLocations = new(), junkLocations = new(), bossKeyLocations = new();
            CreateLocationPool(progressionLocations, junkLocations, bossKeyLocations, settings);

            // Create pools of all items to randomize
            List<Item> progressionItems = new(), junkItems = new();
            CreateItemPool(progressionItems, junkItems, progressionLocations.Count + junkLocations.Count, settings);

            // Place boss key items at boss key locations
            Item bossKeyItem = Main.Randomizer.Data.GetItem("BK");
            FillBossKeyItems(bossKeyLocations, bossKeyItem, output);

            // Place progression items at progression locations
            FillProgressionItems(progressionLocations, progressionItems, bossKeyLocations, output, inventory);

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
        /// Adds the starting weapon to the initial inventory
        /// </summary>
        private void AddStartingItemsToInventory(Blas2Inventory inventory, RandomizerSettings settings)
        {
            // Add the starting weapon
            inventory.AddItem(Main.Randomizer.Data.GetItem(GetStartingWeaponId(settings)));
        }

        /// <summary>
        /// Fills the three location pools
        /// </summary>
        private void CreateLocationPool(List<ItemLocation> progressionLocations, List<ItemLocation> junkLocations, List<ItemLocation> bossKeyLocations, RandomizerSettings settings)
        {
            foreach (var location in Main.Randomizer.Data.GetAllItemLocations())
            {
                AddLocationToPool(progressionLocations, junkLocations, bossKeyLocations, location, settings);
            }
        }

        /// <summary>
        /// Takes a single location data and adds it to the correct list based on its type
        /// </summary>
        private void AddLocationToPool(List<ItemLocation> progressionLocations, List<ItemLocation> junkLocations, List<ItemLocation> bossKeyLocations, ItemLocation location, RandomizerSettings settings)
        {
            if (location.type == ItemLocation.ItemLocationType.BossKey)
            {
                // Boss keys go to special list
                bossKeyLocations.Add(location);
            }
            else if (location.ShouldBeShuffled(settings))
            {
                // Only locations that should be shuffled will have progression items
                progressionLocations.Add(location);
            }
            else
            {
                // Everything else can only have junk
                junkLocations.Add(location);
            }
        }

        /// <summary>
        /// Fills the two item pools to match the number of locations
        /// </summary>
        private void CreateItemPool(List<Item> progressionItems, List<Item> junkItems, int numOfLocations, RandomizerSettings settings)
        {
            foreach (var item in Main.Randomizer.Data.GetAllItems())
            {
                AddItemToPool(progressionItems, junkItems, item);
            }

            RemoveStartingItemsFromItemPool(progressionItems, settings);
            BalanceItemPool(progressionItems, junkItems, numOfLocations);
        }

        /// <summary>
        /// Takes a single item data and adds it to the correct list based on its count
        /// </summary>
        private void AddItemToPool(List<Item> progressionItems, List<Item> junkItems, Item item)
        {
            // Add the item to either progression or junk lists
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
        /// Calculates a subset of the given locations that are reachable with the current inventory
        /// </summary>
        private List<ItemLocation> FindReachableLocations(List<ItemLocation> locations, Blas2Inventory inventory)
        {
            var reachableLocations = new List<ItemLocation>();
            foreach (var location in locations)
            {
                if (inventory.Evaluate(location.logic))
                    reachableLocations.Add(location);
            }
            return reachableLocations;
        }

        /// <summary>
        /// Every cycle through the progression fill, check if boss key locations are reachable and add them to inventory
        /// </summary>
        private void CheckBossKeyLocations(List<ItemLocation> bossKeyLocations, Blas2Inventory inventory)
        {
            List<ItemLocation> reachableLocations = FindReachableLocations(bossKeyLocations, inventory);
            foreach (var location in reachableLocations)
            {
                bossKeyLocations.Remove(location);
                inventory.AddItem(Main.Randomizer.Data.GetItem("BK"));
            }
        }

        /// <summary>
        /// After shuffling the list of progression items, move the wall climb ability to the end to prevent failing seeds
        /// </summary>        
        private void MovePriorityItems(List<Item> progressionItems)
        {
            Item lance = Main.Randomizer.Data.GetItem("QI70");
            progressionItems.Remove(lance);
            progressionItems.Add(lance);

            Item wallClimb = Main.Randomizer.Data.GetItem("AB44");
            progressionItems.Remove(wallClimb);
            progressionItems.Add(wallClimb);
        }

        /// <summary>
        /// Calculates the item id of the chosen starting weapon
        /// </summary>
        private string GetStartingWeaponId(RandomizerSettings settings)
        {
            return "WE0" + (settings.RealStartingWeapon + 1);
        }


        private void FillBossKeyItems(List<ItemLocation> locations, Item item, Dictionary<string, string> output)
        {
            // Remove extra locations until there are only five
            while (locations.Count > 5)
            {
                RemoveRandom(locations);
            }

            // Place boss key item at remaining ones
            foreach (var location in locations)
            {
                Main.Randomizer.Log("Placing boss key at: " + location.id);
                output.Add(location.id, item.id);
            }
        }

        private void FillProgressionItems(List<ItemLocation> locations, List<Item> items, List<ItemLocation> bossKeyLocations, Dictionary<string, string> output, Blas2Inventory inventory)
        {
            ShuffleList(items);
            MovePriorityItems(items);
            List<ItemLocation> reachableLocations = FindReachableLocations(locations, inventory);

            while (reachableLocations.Count > 0 && items.Count > 0)
            {
                ItemLocation location = RemoveRandomFromOther(reachableLocations, locations);
                Item item = RemoveLast(items);

                inventory.AddItem(item);
                output.Add(location.id, item.id);
                Main.Randomizer.Log($"Placing prog item {item.id} at: {location.id}");

                CheckBossKeyLocations(bossKeyLocations, inventory);
                reachableLocations = FindReachableLocations(locations, inventory);
            }
        }

        private void FillJunkItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        {
            ShuffleList(items);

            while (locations.Count > 0 && items.Count > 0)
            {
                ItemLocation location = RemoveLast(locations);
                Item item = RemoveLast(items);

                output.Add(location.id, item.id);
                Main.Randomizer.Log($"Placing junk item {item.id} at: {location.id}");
            }
        }
    }
}
