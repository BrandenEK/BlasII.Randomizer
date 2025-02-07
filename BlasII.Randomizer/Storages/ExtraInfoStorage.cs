using BlasII.Randomizer.Models;
using System.Linq;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores extra info for the Randomizer
/// </summary>
public class ExtraInfoStorage
{
    // Boss teleports

    private readonly BossTeleportInfo[] _bossTeleports =
    [
        new BossTeleportInfo("Z0421", 129108797, "Z0421", 685874534, true),
        new BossTeleportInfo("Z0730", -1436975306, "Z0730", 685874534, true),
        new BossTeleportInfo("Z0921", 129108570, "Z0921", -777454601, true),
        new BossTeleportInfo("Z2304", 1574233179, "Z2304", 1157051513, false), // Causes missing fog vfx
        new BossTeleportInfo("Z1113", 0, "Z1104", 1928462977, false), // Benedicta changes rooms
        new BossTeleportInfo("Z1216", -133013164, "Z1216", -1794395, true),
        new BossTeleportInfo("Z1622", 1433070649, "Z1622", 887137572, true),
        new BossTeleportInfo("Z1327", 1433070681, "Z1327", -284092948, true)
    ];

    /// <summary>
    /// Retrieves a <see cref="BossTeleportInfo"/> by ExitScene
    /// </summary>
    public bool TryGetBossTeleportInfo(string scene, out BossTeleportInfo result)
    {
        return (result = _bossTeleports.FirstOrDefault(x => x.ExitScene == scene)) != null;
    }

    /// <summary>
    /// Retrieves a <see cref="BossTeleportInfo"/> by ExitSceneHash
    /// </summary>
    public bool TryGetBossTeleportInfo(int hash, out BossTeleportInfo result)
    {
        return (result = _bossTeleports.FirstOrDefault(x => x.ExitSceneHash == hash)) != null;
    }

    // Quest bypasses

    private readonly QuestBypassInfo[] _questBypasses =
    [
        // Incense of the Envoys
        new QuestBypassInfo("Z1064", "QI69", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1064.i0")),
        // Chime Prayer
        new QuestBypassInfo("Z1421", "PR03", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1421.l1")),
        // Lullaby of the Shore
        new QuestBypassInfo("Z1906", "PR16", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1906.i1")),

        // Honey Maiden
        new QuestBypassInfo("Z0815", "FG34", () => false),
        new QuestBypassInfo("Z0815", "FG35", () => false),
        new QuestBypassInfo("Z0815", "FG36", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z0815.i0")),
        new QuestBypassInfo("Z0815", "FG37", () => false),
        new QuestBypassInfo("Z0815", "FG38", () => false),
        new QuestBypassInfo("Z0815", "FG39", () => false),

        // Cursed Letter #1
        new QuestBypassInfo("Z1326", "PR15", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1326.i0")),
        new QuestBypassInfo("Z1326", "QI15", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1326.i0")),
        new QuestBypassInfo("Z1326", "QI16", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1326.i0")),
        // Cursed Letter #2
        new QuestBypassInfo("Z0502", "PR15", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z0502.i1")),
        // Cursed Letter #4
        new QuestBypassInfo("Z0503", "PR15", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z0503.i9")),
        new QuestBypassInfo("Z0503", "QI21", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z0503.i9")),
        new QuestBypassInfo("Z0503", "QI22", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z0503.i9")),
        // Cursed Letter #5
        new QuestBypassInfo("Z1917", "PR15", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z1917.i0")),

        // Lacrimatorio #1
        new QuestBypassInfo("Z2702", "QI106", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2702.i0")),
        new QuestBypassInfo("Z2702", "QI107", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2702.i0")),
        new QuestBypassInfo("Z2702", "QI108", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2702.i0")),
        new QuestBypassInfo("Z2702", "QI109", () => false),
        // Lacrimatorio #2
        new QuestBypassInfo("Z2705", "QI106", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2705.i6")),
        new QuestBypassInfo("Z2705", "QI107", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2705.i6")),
        new QuestBypassInfo("Z2705", "QI108", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2705.i6")),
        new QuestBypassInfo("Z2705", "QI109", () => false),
        // Lacrimatorio #3
        new QuestBypassInfo("Z2816", "QI106", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2816.i7")),
        new QuestBypassInfo("Z2816", "QI107", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2816.i7")),
        new QuestBypassInfo("Z2816", "QI108", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2816.i7")),
        new QuestBypassInfo("Z2816", "QI109", () => false),
        // Lacrimatorio #4
        new QuestBypassInfo("Z2828", "QI106", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2828.i17")),
        new QuestBypassInfo("Z2828", "QI107", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2828.i17")),
        new QuestBypassInfo("Z2828", "QI108", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2828.i17")),
        new QuestBypassInfo("Z2828", "QI109", () => false),
        // Lacrimatorio #5
        new QuestBypassInfo("Z2746", "QI110", () => Enumerable.Range(1, 4).All(x => Main.Randomizer.GetQuestBool("ST105", $"TOMB{x}_FINISHED"))),
        new QuestBypassInfo("Z2746", "QI111", () => Main.Randomizer.ItemHandler.IsLocationCollected("Z2746.i0")),
    ];

    /// <summary>
    /// Retrieves a <see cref="QuestBypassInfo"/> with the specified scene and item
    /// </summary>
    public bool TryGetQuestBypassInfo(string scene, string item, out QuestBypassInfo result)
    {
        return (result = _questBypasses.FirstOrDefault(x => x.Scene == scene && x.Item == item)) != null;
    }
}
