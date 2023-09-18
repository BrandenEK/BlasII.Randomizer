using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.Randomizer.Items
{
    internal class ItemShuffler
    {
        public bool Shuffle(uint seed, Dictionary<string, string> mappedItems)
        {
            mappedItems.Clear();

            // Create list of all locations to randomize
            var locations = new List<ItemLocation>();
            CreateLocationPool(locations);

            // Create list of all items to randomize
            var items = new List<Item>();
            CreateItemPool(items, locations.Count);

            // Place an item at a location until both empty
            PlaceItemsAtLocations(locations, items, mappedItems);

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

        private void PlaceItemsAtLocations(List<ItemLocation> locations, List<Item> items, Dictionary<string, string> map)
        {

        }
    }
}
