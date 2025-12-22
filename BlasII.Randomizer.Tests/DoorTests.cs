using BlasII.Randomizer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class DoorTests
{
    [TestMethod]
    public void TestValidDirections()
    {
        var directions = new Dictionary<Door.DoorDirection, int>();

        foreach (var door in DataStorage.Doors)
        {
            Door.DoorDirection d = door.Direction;

            if (!directions.ContainsKey(d))
                directions.Add(d, 1);
            else
                directions[d]++;
        }

        throw new System.Exception(string.Join(',', directions.Select(x => $"{x.Key}: {x.Value}")));
    }
}
