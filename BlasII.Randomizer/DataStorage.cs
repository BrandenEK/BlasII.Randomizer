using BlasII.Randomizer.Doors;
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
        public Item InvalidItem => _allItems["INVALID"];

        // Item locations
        private readonly Dictionary<string, ItemLocation> _allItemLocations = new();

        public ItemLocation GetItemLocation(string id) => _allItemLocations.TryGetValue(id, out var itemLocation) ? itemLocation : null;
        public bool DoesItemLocationExist(string id) => _allItemLocations.ContainsKey(id);
        public IEnumerable<ItemLocation> GetAllItemLocations() => _allItemLocations.Values;

        // Doors
        private readonly Dictionary<string, DoorLocation> _allDoors = new();

        public DoorLocation GetDoor(string id) => _allDoors.TryGetValue(id, out var door) ? door : null;
        public bool DoesDoorExist(string id) => _allDoors.ContainsKey(id);
        public IEnumerable<DoorLocation> GetAllDoors() => _allDoors.Values;

        // Images

        private readonly Dictionary<ImageType, Sprite> _images = new();
        private readonly Dictionary<UIType, Sprite> _ui = new();

        public Sprite GetImage(ImageType type) => _images.TryGetValue(type, out var sprite) ? sprite : null;
        public Sprite GetUI(UIType type) => _ui.TryGetValue(type, out var sprite) ? sprite : null;

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

            if (Main.Randomizer.FileHandler.LoadDataAsJson("doors.json", out DoorLocation[] doors))
            {
                foreach (var door in doors)
                    _allDoors.Add(door.id, door);
            }
            Main.Randomizer.Log($"Loaded {_allDoors.Count} doors!");
        }

        private void LoadAllImages()
        {
            if (Main.Randomizer.FileHandler.LoadDataAsTexture("rando-items.png", out Sprite[] images, 30, 32, true))
            {
                for (int i = 0; i < images.Length; i++)
                    _images.Add((ImageType)i, images[i]);
            }
            Main.Randomizer.Log($"Loaded {_images.Count} images!");

            if (Main.Randomizer.FileHandler.LoadDataAsTexture("ui.png", out Sprite[] ui, 36, 36, true))
            {
                for (int i = 0; i < ui.Length; i++)
                    _ui.Add((UIType)i, ui[i]);
            }
            Main.Randomizer.Log($"Loaded {_ui.Count} ui elements!");
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
            Invalid = 9,
        }

        public enum UIType
        {
            ToggleOff = 0,
            ToggleOn = 1,
            ToggleNo = 2,
            ArrowLeftOn = 3,
            ArrowRightOn = 4,
            ArrowLeftOff = 5,
            ArrowRightOff = 6,
            TextInactive = 7,
            TextActive = 8,
        }

        // Special data

        private readonly Dictionary<string, string> _zoneNames = new()
        {
            { "Z01", "Repose of the Silent One" },
            { "Z02", "Ravine of the High Stones" },
            { "Z03", "Aqueduct of the Costales" },
            { "Z04", "Sacred Entombments" },
            { "Z05", "City of the Blessed Name" },
            { "Z06", "Grilles and Ruins" },
            { "Z07", "Palace of the Embroideries" },
            { "Z08", "Choir of Thorns" },
            { "Z09", "Crown of Towers" },
            { "Z10", "Elevated Temples" },
            { "Z11", "Basilica of Absent Faces" },
            { "Z12", "Sunken Cathedral" },
            { "Z13", "Two Moons" },
            { "Z14", "Mother of Mothers" },
            { "Z15", "Dreams of Incense" },
            { "Z16", "The Severed Tower" },
            { "Z17", "Streets of Wakes" },
            { "Z18", "Crimson Rains" },
            { "Z19", "Profundo Lamento" },
            { "Z20", "Sea of Ink" },
            { "Z21", "Labyrinth of Tides" },
            { "Z23", "Beneath Her Sacred Grounds" },
            { "Z24", "Garden of the High Choirs" },
            { "SHO", "Shops" },
        };

        public bool GetZoneName(string zone, out string name)
        {
            return _zoneNames.TryGetValue(zone, out name);
        }
    }
}
