using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using System.Collections.Generic;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Contains lists of grouped items
/// </summary>
public static class ItemGroups
{
    public static string[] Lullabies { get; } = ["QI23", "QI24", "QI25", "QI26", "QI27"];

    public static string[] Lacrimatorios { get; } = ["QI106", "QI107", "QI108", "QI110", "QI111"];

    public static string[] SculptorTools { get; } = ["QI12", "QI02", "QI11", "QI03", "QI01"];

    public static string[] ChallengeRewards { get; } = ["PR103", "PR108", "FG101", "FG105", "FG106", "FG111"];

    public static ProgressiveItemData NewLullabies = new ProgressiveItemData("UL",
        new ProgressiveItemLevel("QI23"),
        new ProgressiveItemLevel("QI24", new RemoveItem("QI23")),
        new ProgressiveItemLevel("QI25", new RemoveItem("QI24")),
        new ProgressiveItemLevel("QI26", new RemoveItem("QI25")),
        new ProgressiveItemLevel("QI27", new RemoveItem("QI26")));

    public static ProgressiveItemData NewSculptorTools = new ProgressiveItemData("ST",
        new ProgressiveItemLevel("QI12"),
        new ProgressiveItemLevel("QI02"),
        new ProgressiveItemLevel("QI11"),
        new ProgressiveItemLevel("QI03"),
        new ProgressiveItemLevel("QI01"));

    public static ProgressiveItemData NewLacrimatorios = new ProgressiveItemData("IL",
        new ProgressiveItemLevel("QI106"),
        new ProgressiveItemLevel("QI107", new RemoveItem("QI106")),
        new ProgressiveItemLevel("QI108", new RemoveItem("QI107")),
        new ProgressiveItemLevel("QI110", new RemoveItem("QI108")),
        new ProgressiveItemLevel("QI111", new RemoveItems("QI109", "QI110"), new GrantAbility(ABILITY_IDS.GoldFlask)));
}

public class ProgressiveItemData(string id, params ProgressiveItemLevel[] levels)
{
    public string Id { get; } = id;

    private readonly ProgressiveItemLevel[] _levels = levels;

    public ProgressiveItemLevel CurrentProgressiveLevel
    {
        get
        {
            int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id) - 1;
            return level >= 0 && level < _levels.Length ? _levels[level] : new ProgressiveItemLevel("INVALID");
        }
    }

    public ProgressiveItemLevel NextProgressiveLevel
    {
        get
        {
            int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id);
            return level >= 0 && level < _levels.Length ? _levels[level] : new ProgressiveItemLevel("INVALID");
        }
    }
}

public class ProgressiveItemLevel(string id, params List<IFunction> functions)
{
    public string Id { get; } = id;

    private readonly List<IFunction> _functions = functions;

    public void PerformFunctions()
    {
        foreach (var function in _functions)
            function.Execute();
    }

    public bool IsValid => Id != "INVALID";
}

public interface IFunction
{
    public void Execute();
}

public class RemoveItem(string item) : IFunction
{

    public void Execute()
    {
        ModLog.Warn("Removing item " + item);
        AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[item]);
    }
}

public class RemoveItems(params List<string> items) : IFunction
{
    public void Execute()
    {
        foreach (string item in items)
        {
            ModLog.Warn("Removing item " + item);
            AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[item]);
        }
    }
}

public class GrantAbility(ABILITY_IDS ability) : IFunction
{
    public void Execute()
    {
        ModLog.Warn($"Granting ability {ability}");
        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ability], true);
    }
}
