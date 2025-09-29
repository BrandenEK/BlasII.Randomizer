using BlasII.Randomizer.Settings;
using BlasII.Randomizer.Shuffle;
using Basalt.LogicParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace BlasII.Randomizer.Tests;

[TestClass]
public class LogicTests
{
    private GameInventory inventory;

    [TestInitialize]
    public void CreateInventory()
    {
        inventory = BlasphemousInventory.CreateNewInventory(SettingsGenerator.CreateFromPreset(Preset.Standard));
    }

    //[TestMethod]
    //public void FindErrorsInDoorLogic()
    //{
    //    var sb = new StringBuilder(Environment.NewLine);
    //    bool invalid = false;

    //    foreach (var door in _allDoors.Values)
    //    {
    //        try
    //        {
    //            inventory.Evaluate(door.Logic);
    //        }
    //        catch (LogicParserException e)
    //        {
    //            sb.AppendLine($"[{door.Id}] {e.Message}");
    //            invalid = true;
    //        }
    //    }

    //    if (invalid)
    //    {
    //        throw new LogicParserException(sb.ToString());
    //    }
    //}

    [TestMethod]
    public void FindErrorsInLocationLogic()
    {
        var sb = new StringBuilder(Environment.NewLine);
        bool invalid = false;

        foreach (var itemLocation in DataStorage.ItemLocations)
        {
            try
            {
                inventory.Evaluate(itemLocation.Logic);
            }
            catch (LogicParserException e)
            {
                sb.AppendLine($"[{itemLocation.Id}] {e.Message}");
                invalid = true;
            }
        }

        if (invalid)
        {
            throw new LogicParserException(sb.ToString());
        }
    }
}