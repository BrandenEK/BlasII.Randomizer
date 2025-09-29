using BlasII.Randomizer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class DataStorage
{
    private static readonly Dictionary<string, Item> _items = new();
    private static readonly Dictionary<string, ItemLocation> _itemLocations = new();
    private static readonly Dictionary<string, Door> _doors = new();

    public static IEnumerable<Item> Items => _items.Values;
    public static IEnumerable<ItemLocation> ItemLocations => _itemLocations.Values;
    public static IEnumerable<Door> Doors => _doors.Values;

    [AssemblyInitialize]
    public static void LoadJsonData(TestContext _)
    {
        string dataFolder = "../../../../resources/data/Randomizer/";

        string items = File.ReadAllText(dataFolder + "items.json");
        foreach (var item in JsonConvert.DeserializeObject<Item[]>(items))
            _items.Add(item.Id, item);

        string itemLocations = File.ReadAllText(dataFolder + "itemlocations.json");
        foreach (var itemLocation in JsonConvert.DeserializeObject<ItemLocation[]>(itemLocations))
            _itemLocations.Add(itemLocation.Id, itemLocation);

        string doors = File.ReadAllText(dataFolder + "doors.json");
        foreach (var door in JsonConvert.DeserializeObject<Door[]>(doors))
            _doors.Add(door.Id, door);
    }
}
