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
            var locations = new List<ItemLocation>();
            CreateLocationPool(locations);

            // Create list of all items to randomize
            var items = new List<Item>();
            CreateItemPool(items, locations.Count);

            // Place an item at a location until both empty
            PlaceItemsAtLocations(locations, items, output);

            return locations.Count == 0 && items.Count == 0;
        }

        private void CreateLocationPool(List<ItemLocation> locations)
        {
            foreach (var location in Main.Randomizer.Data.GetAllItemLocations())
            {
                locations.Add(location);
            }
        }

        private void CreateItemPool(List<Item> items, int numOfLocations)
        {
            foreach (var item in Main.Randomizer.Data.GetAllItems())
            {
                if (item.count == 1)
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
