using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    internal class ItemShuffler : BaseShuffler
    {
        public override bool Shuffle(uint seed, TempConfig config, Dictionary<string, string> output)
        {
            output.Clear();
            Initialize(seed);

            // Create list of all locations to randomize
            List<ItemLocation> locations = new(), bossKeyLocations = new();
            CreateLocationPool(locations, bossKeyLocations);

            // Create list of all items to randomize
            List<Item> items = new(), bossKeyItems = new();
            CreateItemPool(items, bossKeyItems, locations.Count, config);

            // Place a boss key at 5 locations, discard the rest
            PlaceItemsAtLocations(bossKeyLocations, bossKeyItems, output);

            // Place an item at a location until both empty
            PlaceItemsAtLocations(locations, items, output);

            return locations.Count == 0 && items.Count == 0;
        }

        private void CreateLocationPool(List<ItemLocation> locations, List<ItemLocation> bossKeyLocations)
        {
            foreach (var location in Main.Randomizer.Data.GetAllItemLocations())
            {
                if (location.type == ItemLocation.ItemLocationType.BossKey)
                    bossKeyLocations.Add(location);
                else
                    locations.Add(location);
            }
        }

        private void CreateItemPool(List<Item> items, List<Item> bossKeyItems, int numOfLocations, TempConfig config)
        {
            foreach (var item in Main.Randomizer.Data.GetAllItems())
            {
                if (item.type == Item.ItemType.BossKey)
                {
                    bossKeyItems.Add(item);
                }
                else if (item.count == 1)
                {
                    items.Add(item);
                }
                else if (item.count > 1)
                {
                    for (int i = 0; i < item.count; i++)
                    {
                        items.Add(item);
                    }
                }
            }

            // Remove the extra starting weapon
            string startingWeaponId = config.startingWeapon switch
            {
                0 => "WE01",
                1 => "WE04",
                2 => "WE03",
                _ => throw new System.Exception("Invalid starting weapon in the config")
            };
            items.Remove(Main.Randomizer.Data.GetItem(startingWeaponId));

            // Remove tear items until pools are equal
            while (items.Count > numOfLocations)
            {
                items.RemoveAt(items.Count - 1);
            }

            // Add tear items until pools are equal
            while (items.Count < numOfLocations)
            {
                items.Add(Main.Randomizer.Data.GetItem("Tears[800]"));
            }
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
    }
}
