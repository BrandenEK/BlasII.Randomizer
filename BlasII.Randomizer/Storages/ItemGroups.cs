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
        new ProgressiveItemLevel("QI24", new RemovePrevious()),
        new ProgressiveItemLevel("QI25", new RemovePrevious()),
        new ProgressiveItemLevel("QI26", new RemovePrevious()),
        new ProgressiveItemLevel("QI27", new RemovePrevious()));
}

public class ProgressiveItemData(string id, params ProgressiveItemLevel[] levels)
{
    public string Id { get; } = id;

    public ProgressiveItemLevel[] Levels { get; } = levels;
}

public class ProgressiveItemLevel(string id, params List<IFunction> functions)
{
    public string Id { get; } = id;

    public List<IFunction> Functions { get; } = functions;
}

public interface IFunction
{
    public void Grant(ProgressiveItemData data, int current);
}

public class RemovePrevious : IFunction
{
    public void Grant(ProgressiveItemData data, int current)
    {
        // Remove the previous level
    }
}
