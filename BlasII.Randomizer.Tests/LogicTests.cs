using BlasII.Randomizer.Doors;
using BlasII.Randomizer.Items;
using BlasII.Randomizer.Shuffle;
using LogicParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlasII.Randomizer.Tests
{
    [TestClass]
    public class LogicTests
    {
        private static readonly Dictionary<string, Item> _allItems = new();
        private static readonly Dictionary<string, ItemLocation> _allItemLocations = new();
        private static readonly Dictionary<string, DoorLocation> _allDoors = new();

        private Blas2Inventory inventory;

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

        [TestInitialize]
        public void CreateInventory()
        {
            inventory = new Blas2Inventory(RandomizerSettings.DefaultSettings, _allDoors);
        }

        [TestMethod]
        public void FindErrorsInDoorLogic()
        {
            var sb = new StringBuilder(Environment.NewLine);
            bool invalid = false;

            foreach (var door in _allDoors.Values)
            {
                try
                {
                    inventory.Evaluate(door.logic);
                }
                catch (LogicParserException e)
                {
                    sb.AppendLine($"[{door.id}] {e.Message}");
                    invalid = true;
                }
            }

            if (invalid)
            {
                throw new LogicParserException(sb.ToString());
            }
        }

        [TestMethod]
        public void FindErrorsInLocationLogic()
        {
            var sb = new StringBuilder(Environment.NewLine);
            bool invalid = false;

            foreach (var itemLocation in _allItemLocations.Values)
            {
                try
                {
                    inventory.Evaluate(itemLocation.logic);
                }
                catch (LogicParserException e)
                {
                    sb.AppendLine($"[{itemLocation.id}] {e.Message}");
                    invalid = true;
                }
            }

            if (invalid)
            {
                throw new LogicParserException(sb.ToString());
            }
        }
    }
}