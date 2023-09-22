using BlasII.Randomizer.Items;
using System.Collections.Generic;
using UnityEngine;

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

        // Images

        private readonly Dictionary<ImageType, Sprite> _images = new();

        public Sprite GetImage(ImageType type) => _images.TryGetValue(type, out var sprite) ? sprite : null;

        // Loading

        public void Initialize()
        {
            LoadAllJsonData();
            LoadAllImages();
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

        private void LoadAllImages()
        {
            if (Main.Randomizer.FileHandler.LoadDataAsTexture("rando-items.png", out Sprite[] images, 30, 32, true))
            {
                for (int i = 0; i < images.Length; i++)
                {
                    _images.Add((ImageType)i, images[i]);
                }
            }
            Main.Randomizer.Log($"Loaded {_images.Count} images!");
        }

        public enum ImageType
        {
            Cherub = 0,
            WallClimb = 1,
            DoubleJump = 2,
            AirDash = 3,
            CherubRing = 4,
            Censer = 5,
            Blade = 6,
            Rapier = 7,
            Tears = 8,
        }
    }
}
