using BlasII.Randomizer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class DoorTests
{
    private static readonly Dictionary<Door.DoorDirection, int> _directions = new();

    [ClassInitialize]
    public static void SetupDirections(TestContext context)
    {
        foreach (var door in DataStorage.Doors)
        {
            Door.DoorDirection d = door.Direction;

            if (!_directions.ContainsKey(d))
                _directions.Add(d, 1);
            else
                _directions[d]++;
        }
    }

    [TestMethod]
    public void TestValidDirections()
    {
        // Just list all invalid pairs
        throw new System.Exception(string.Join(',', _directions.Select(x => $"{x.Key}: {x.Value}")));
    }
}
