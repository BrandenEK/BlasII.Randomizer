using BlasII.Randomizer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class DoorTests
{
    [TestMethod]
    public void TestDoorMatches()
    {
        var sb = new StringBuilder();
        bool invalid = false;

        foreach (var entrance in DataStorage.Doors)
        {
            Door exit = DataStorage.GetDoor(entrance.Exit);

            // The exit door does not lead back to the entrance
            if (exit.Exit != entrance.Id)
            {
                sb.AppendLine($"{entrance.Id} != {exit.Id}'s exit");
                invalid = true;
                continue;
            }

            // The exit door has different flags
            if (entrance.Flags != exit.Flags)
            {
                sb.AppendLine($"{entrance.Id} is {entrance.Flags}, but {exit.Id} is {exit.Flags}");
                invalid = true;
            }

            // The exit door doesn't have the oppsite direction
            if (entrance.Direction != GetOppositeDirection(exit))
            {
                sb.AppendLine($"{entrance.Id} is {entrance.Direction}, but {exit.Id} is {exit.Direction}");
                invalid = true;
            }
        }

        if (invalid)
            throw new Exception(sb.ToString());
    }

    private static Door.DoorDirection GetOppositeDirection(Door door)
    {
        int direction = (int)door.Direction;
        return (Door.DoorDirection)(direction % 2 == 0 ? direction + 1 : direction - 1);
    }
}
