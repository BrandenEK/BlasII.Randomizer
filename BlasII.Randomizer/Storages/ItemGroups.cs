using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Models;
using Il2CppTGK.Game;
using System.Collections.Generic;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Contains lists of grouped items
/// </summary>
public static class ItemGroups
{
    public static string[] ChallengeRewards { get; } = ["PR103", "PR108", "FG101", "FG105", "FG106", "FG111"];
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

    public bool TryGetNextProgressiveLevel(out ProgressiveItemLevel nextLevel)
    {
        int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id);

        if (level >= 0 && level < _levels.Length)
        {
            nextLevel = _levels[level];
            return true;
        }

        nextLevel = new ProgressiveItemLevel("INVALID");
        return false;
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
