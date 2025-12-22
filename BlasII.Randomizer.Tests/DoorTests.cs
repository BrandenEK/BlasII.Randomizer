using BlasII.Randomizer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class DoorTests
{
    private static readonly Dictionary<Door.DoorDirection, int> _directions = new();
    private static readonly Dictionary<Door.DoorType, int> _types = new();

    [ClassInitialize]
    public static void SetupClass(TestContext context)
    {
        foreach (var door in DataStorage.Doors)
        {
            Door.DoorDirection d = door.Direction;

            if (!_directions.ContainsKey(d))
                _directions.Add(d, 1);
            else
                _directions[d]++;
        }

        foreach (var door in DataStorage.Doors)
        {
            Door.DoorType t = door.Type;

            if (!_types.ContainsKey(t))
                _types.Add(t, 1);
            else
                _types[t]++;
        }
    }

    [TestMethod]
    public void TestEvenNumberOfDoors()
    {
        Assert.IsTrue(DataStorage.Doors.Count() % 2 == 0);
    }

    [TestMethod]
    public void TestEvenNumberOfEachType()
    {
        var sb = new StringBuilder();
        bool invalid = false;

        foreach (var kvp in _types)
        {
            if (kvp.Value % 2 == 0)
                continue;

            sb.AppendLine($"{kvp.Key} has {kvp.Value} doors");
            invalid = true;
        }

        if (invalid)
            throw new System.Exception(sb.ToString());
    }

    [TestMethod]
    public void TestValidDirections()
    {
        // Just list all invalid pairs
        throw new System.Exception(string.Join(',', _directions.Select(x => $"{x.Key}: {x.Value}")));
    }
}
