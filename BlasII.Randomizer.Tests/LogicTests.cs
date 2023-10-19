using BlasII.Randomizer.Doors;
using BlasII.Randomizer.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BlasII.Randomizer.Tests
{
    [TestClass]
    public class LogicTests
    {
        private static readonly Dictionary<string, Item> _allItems = new();
        private static readonly Dictionary<string, ItemLocation> _allItemLocations = new();
        private static readonly Dictionary<string, DoorLocation> _allDoors = new();

        [ClassInitialize]
        public static void LoadJsonData(TestContext context)
        {
            string dataFolder = "../../../../resources/data/Randomizer/";

            string items = File.ReadAllText(dataFolder + "items.json");
            foreach (var item in JsonConvert.DeserializeObject<Item[]>(items))
                _allItems.Add(item.id, item);

            string itemLocations = File.ReadAllText(dataFolder + "item-locations.json");
            foreach (var itemLocation in JsonConvert.DeserializeObject<ItemLocation[]>(itemLocations))
                _allItemLocations.Add(itemLocation.id, itemLocation);

            string doors = File.ReadAllText(dataFolder + "doors.json");
            foreach (var door in JsonConvert.DeserializeObject<DoorLocation[]>(doors))
                _allDoors.Add(door.id, door);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var inventory = new Blas2Inventory();

            Assert.AreEqual(176, _allItems.Count);
        }
    }
}