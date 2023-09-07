using BlasII.Randomizer.Items;
using System.Collections.Generic;

namespace BlasII.Randomizer
{
    public class DataStorage
    {
        // Items
        private readonly Dictionary<string, Item> _allItems = new();

        public Item GetItem(string id) => _allItems.TryGetValue(id, out var item) ? item : null;
        public bool DoesItemExist(string id) => _allItems.ContainsKey(id);
        public IEnumerable<Item> GetAllItems() => _allItems.Values;

        // Item locations
        private readonly Dictionary<string, ItemLocation> _allItemLocations = new();

        public ItemLocation GetItemLocation(string id) => _allItemLocations.TryGetValue(id, out var itemLocation) ? itemLocation : null;
        public bool DoesItemLocationExist(string id) => _allItemLocations.ContainsKey(id);
        public IEnumerable<ItemLocation> GetAllItemLocations() => _allItemLocations.Values;

        // Loading

        public void Initialize()
        {
            LoadAllJsonData();
        }

        private void LoadAllJsonData()
        {
            if (Main.Randomizer.FileHandler.LoadDataAsJson("items.json", out Item[] items))
            {
                foreach (var item in items)
                    _allItems.Add(item.id, item);
            }
            Main.Randomizer.Log($"Loaded {_allItems.Count} items!");

            if (Main.Randomizer.FileHandler.LoadDataAsJson("item-locations.json", out ItemLocation[] itemLocations))
            {
                foreach (var itemLocation in itemLocations)
                    _allItemLocations.Add(itemLocation.id, itemLocation);
            }
            Main.Randomizer.Log($"Loaded {_allItemLocations.Count} item locations!");
        }
    }
}
