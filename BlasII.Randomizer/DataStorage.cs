using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using BlasII.Randomizer.Doors;
using BlasII.Randomizer.Items;
using BlasII.Randomizer.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer;

public class DataStorage
{
    // Items
    private readonly Dictionary<string, Item> _allItems = new();

    public IEnumerable<Item> ItemList => _allItems.Values;
    public IDictionary<string, Item> ItemDictionary => _allItems;

    public Item GetItem(string id) => _allItems.TryGetValue(id, out var item) ? item : null;
    public bool DoesItemExist(string id) => _allItems.ContainsKey(id);
    public Item InvalidItem => _allItems["INVALID"];

    // Item locations
    private readonly Dictionary<string, ItemLocation> _allItemLocations = new();

    public IEnumerable<ItemLocation> ItemLocationList => _allItemLocations.Values;
    public IDictionary<string, ItemLocation> ItemLocationDictionary => _allItemLocations;

    public ItemLocation GetItemLocation(string id) => _allItemLocations.TryGetValue(id, out var itemLocation) ? itemLocation : null;
    public bool DoesItemLocationExist(string id) => _allItemLocations.ContainsKey(id);

    // Doors
    private readonly Dictionary<string, DoorLocation> _allDoors = new();

    public IEnumerable<DoorLocation> DoorList => _allDoors.Values;
    public IDictionary<string, DoorLocation> DoorDictionary => _allDoors;

    public DoorLocation GetDoor(string id) => _allDoors.TryGetValue(id, out var door) ? door : null;
    public bool DoesDoorExist(string id) => _allDoors.ContainsKey(id);

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
        ModLog.Info($"Loaded {_allItems.Count} items!");

        if (Main.Randomizer.FileHandler.LoadDataAsJson("item-locations.json", out ItemLocation[] itemLocations))
        {
            foreach (var itemLocation in itemLocations)
                _allItemLocations.Add(itemLocation.id, itemLocation);
        }
        ModLog.Info($"Loaded {_allItemLocations.Count} item locations!");

        if (Main.Randomizer.FileHandler.LoadDataAsJson("doors.json", out DoorLocation[] doors))
        {
            foreach (var door in doors)
                _allDoors.Add(door.id, door);
        }
        ModLog.Info($"Loaded {_allDoors.Count} doors!");
    }

    private void LoadAllImages()
    {
        Main.Randomizer.FileHandler.LoadDataAsFixedSpritesheet("rando-items.png", new Vector2(30, 30),
            out Sprite[] images, new SpriteImportOptions() { PixelsPerUnit = 32 });

        for (int i = 0; i < images.Length; i++)
            _images.Add((ImageType)i, images[i]);
        ModLog.Info($"Loaded {_images.Count} images!");
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
        Chest = 10,
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

    private readonly BossTeleportInfo[] _bossTeleports =
    [
        new BossTeleportInfo("Z0421", 129108797, "Z0421", 685874534, true),
        new BossTeleportInfo("Z0730", -1436975306, "Z0730", 685874534, true),
        new BossTeleportInfo("Z0921", 129108570, "Z0921", -777454601, true),
        new BossTeleportInfo("Z2304", 1574233179, "Z2304", 1157051513, false), // Causes missing fog vfx
        new BossTeleportInfo("Z1113", 0, "Z1104", 1928462977, false), // Benedicta changes rooms
        new BossTeleportInfo("Z1216", -133013164, "Z1216", -1794395, true),
        new BossTeleportInfo("Z1622", 1433070649, "Z1622", 887137572, true),
        new BossTeleportInfo("Z1327", 1433070681, "Z1327", -284092948, true)
    ];

    /// <summary>
    /// Retrieves a <see cref="BossTeleportInfo"/> by ExitScene
    /// </summary>
    public bool TryGetBossTeleportInfo(string scene, out BossTeleportInfo result)
    {
        return (result = _bossTeleports.FirstOrDefault(x => x.ExitScene == scene)) != null;
    }

    /// <summary>
    /// Retrieves a <see cref="BossTeleportInfo"/> by ExitSceneHash
    /// </summary>
    public bool TryGetBossTeleportInfo(int hash, out BossTeleportInfo result)
    {
        return (result = _bossTeleports.FirstOrDefault(x => x.ExitSceneHash == hash)) != null;
    }
}
