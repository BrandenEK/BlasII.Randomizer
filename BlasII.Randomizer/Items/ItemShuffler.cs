using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    internal class ItemShuffler : BaseShuffler
    {
        public override bool Shuffle(uint seed, TempConfig config, Dictionary<string, string> output)
        {
            output.Clear();
            Initialize(seed);

            // Create pools of all locations to randomize
            List<ItemLocation> mainLocations = new(), bossKeyLocations = new();
            CreateLocationPool(mainLocations, bossKeyLocations);

            // Create pools of all items to randomize
            List<Item> progressionItems = new(), junkItems = new(), bossKeyItems = new();
            CreateItemPool(progressionItems, junkItems, bossKeyItems, mainLocations.Count, config);

            // Place boss key items at boss key locations
            FillBossKeyItems(bossKeyLocations, bossKeyItems, output);

            // Place progression items at main locations
            FillProgressionItems(mainLocations, progressionItems, output, inventory);

            // Place junk items at main locations
            FillJunkItems(mainLocations, junkItems, output);

            return false;
        }

        /// <summary>
        /// Fills the two location pools
        /// </summary>
        private void CreateLocationPool(List<ItemLocation> mainLocations, List<ItemLocation> bossKeyLocations)
        {
            foreach (var location in Main.Randomizer.Data.GetAllItemLocations())
            {
                AddLocationToPool(mainLocations, bossKeyLocations, location);
            }
        }

        /// <summary>
        /// Takes a single location data and adds it to the correct list based on its id
        /// </summary>
        private void AddLocationToPool(List<ItemLocation> mainLocations, List<ItemLocation> bossKeyLocations, ItemLocation location)
        {
            // Add the location to either main or boss key lists
            List<ItemLocation> locationPool = location.id.EndsWith(".key") ? bossKeyLocations : mainLocations;
            locationPool.Add(location);
        }

        /// <summary>
        /// Fills the three item pools to match the number of locations
        /// </summary>
        private void CreateItemPool(List<Item> progressionItems, List<Item> junkItems, List<Item> bossKeyItems, int numOfLocations, TempConfig config)
        {
            foreach (var item in Main.Randomizer.Data.GetAllItems())
            {
                AddItemToPool(progressionItems, junkItems, bossKeyItems, item);
            }

            RemoveStartingItemsFromItemPool(progressionItems, config);
            BalanceItemPool(progressionItems, junkItems, numOfLocations);
        }

        /// <summary>
        /// Takes a single item data and adds it to the correct list based on its count
        /// </summary>
        private void AddItemToPool(List<Item> progressionItems, List<Item> junkItems, List<Item> bossKeyItems, Item item)
        {
            // Add boss keys to separate list
            if (item.id == "BK")
            {
                bossKeyItems.AddRange(new Item[] { item, item, item, item, item });
                return;
            }

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
        private void RemoveStartingItemsFromItemPool(List<Item> items, TempConfig config)
        {
            // Remove the extra starting weapon
            items.Remove(Main.Randomizer.Data.GetItem(GetStartingWeaponId(config)));
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
        /// Calculates the item id of the chosen starting weapon
        /// </summary>
        private string GetStartingWeaponId(TempConfig config)
        {
            return "WE0" + (config.startingWeapon + 1);
        }





        private void PlaceItemsAtLocations(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        {
            ShuffleList(items);

            while (locations.Count > 0 && items.Count > 0)
            {
                int locationIdx = RandomInteger(locations.Count);
                int itemIdx = items.Count - 1;

                Main.Randomizer.LogWarning($"Placing {items[itemIdx].id} at {locations[locationIdx].id}");
                output.Add(locations[locationIdx].id, items[itemIdx].id);

                locations.RemoveAt(locationIdx);
                items.RemoveAt(itemIdx);
            }
        }

        private void FillBossKeyItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        {

        }

        private void FillProgressionItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output, Blas2Inventory inventory)
        {

        }

        private void FillJunkItems(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> output)
        {

        }
    }
}
