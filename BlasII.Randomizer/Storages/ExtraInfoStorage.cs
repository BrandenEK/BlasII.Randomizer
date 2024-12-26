using BlasII.Randomizer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores extra info for the Randomizer
/// </summary>
public class ExtraInfoStorage
{
    // Zone names

    private readonly Dictionary<string, string> _zoneNames = new()
    {
        { "Z01", "Repose of the Silent One" },
        { "Z02", "Ravine of the High Stones" },
        { "Z03", "Aqueduct of the Costales" },
        { "Z04", "Sacred Entombments" },
        { "Z05", "City of the Blessed Name" },
        { "Z06", "Grilles and Ruins" },
        { "Z07", "Palace of the Embroideries" },
        { "Z08", "Choir of Thorns" },
        { "Z09", "Crown of Towers" },
        { "Z10", "Elevated Temples" },
        { "Z11", "Basilica of Absent Faces" },
        { "Z12", "Sunken Cathedral" },
        { "Z13", "Two Moons" },
        { "Z14", "Mother of Mothers" },
        { "Z15", "Dreams of Incense" },
        { "Z16", "The Severed Tower" },
        { "Z17", "Streets of Wakes" },
        { "Z18", "Crimson Rains" },
        { "Z19", "Profundo Lamento" },
        { "Z20", "Sea of Ink" },
        { "Z21", "Labyrinth of Tides" },
        { "Z23", "Beneath Her Sacred Grounds" },
        { "Z24", "Garden of the High Choirs" },
        { "Z27", "Icebound Mausoleum" },
        { "Z28", "Santa Vigilia" },
        { "SHO", "Shops" },
    };

    /// <summary>
    /// Retrieves the name of a zone by the Id
    /// </summary>
    public bool GetZoneName(string zone, out string name)
    {
        return _zoneNames.TryGetValue(zone, out name);
    }

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
        new QuestBypassInfo("Z0502", "PR15", "Z0502.i1"), // Cursed Letter #2
        new QuestBypassInfo("Z0503", "PR15", "Z0503.i9"), // Cursed Letter #4
        new QuestBypassInfo("Z0503", "QI21", "Z0503.i9"), // Cursed Letter #4
        new QuestBypassInfo("Z0503", "QI22", "Z0503.i9"), // Cursed Letter #4
        new QuestBypassInfo("Z1064", "QI69", "Z1064.i0"), // Incense of Envoys
        new QuestBypassInfo("Z1326", "PR15", "Z1326.i0"), // Cursed Letter #1
        new QuestBypassInfo("Z1326", "QI15", "Z1326.i0"), // Cursed Letter #1
        new QuestBypassInfo("Z1326", "QI16", "Z1326.i0"), // Cursed Letter #1
        new QuestBypassInfo("Z1421", "PR03", "Z1421.l1"), // Chime Prayer
        new QuestBypassInfo("Z1906", "PR16", "Z1906.i1"), // Lullaby of the Shore
        new QuestBypassInfo("Z1917", "PR15", "Z1917.i0"), // Cursed Letter #5
        new QuestBypassInfo("Z2702", "QI106", "Z2702.i0"), // Lacrimatorio #1
        new QuestBypassInfo("Z2702", "QI107", "Z2702.i0"), // Lacrimatorio #1
        new QuestBypassInfo("Z2702", "QI108", "Z2702.i0"), // Lacrimatorio #1
        new QuestBypassInfo("Z2702", "QI109", "--------"), // Lacrimatorio #1
        new QuestBypassInfo("Z2705", "QI106", "Z2705.i6"), // Lacrimatorio #2
        new QuestBypassInfo("Z2705", "QI107", "Z2705.i6"), // Lacrimatorio #2
        new QuestBypassInfo("Z2705", "QI108", "Z2705.i6"), // Lacrimatorio #2
        new QuestBypassInfo("Z2705", "QI109", "--------"), // Lacrimatorio #2
        new QuestBypassInfo("Z2816", "QI106", "Z2816.i7"), // Lacrimatorio #3
        new QuestBypassInfo("Z2816", "QI107", "Z2816.i7"), // Lacrimatorio #3
        new QuestBypassInfo("Z2816", "QI108", "Z2816.i7"), // Lacrimatorio #3
        new QuestBypassInfo("Z2816", "QI109", "--------"), // Lacrimatorio #3
        new QuestBypassInfo("Z2828", "QI106", "Z2828.i17"), // Lacrimatorio #4
        new QuestBypassInfo("Z2828", "QI107", "Z2828.i17"), // Lacrimatorio #4
        new QuestBypassInfo("Z2828", "QI108", "Z2828.i17"), // Lacrimatorio #4
        new QuestBypassInfo("Z2828", "QI109", "---------"), // Lacrimatorio #4
        new QuestBypassInfo("Z2746", "QI111", "Z2746.i0"), // Lacrimatorio #5
    ];

    /// <summary>
    /// Retrieves a <see cref="QuestBypassInfo"/> with the specified scene and item
    /// </summary>
    public bool TryGetQuestBypassInfo(string scene, string item, out QuestBypassInfo result)
    {
        return (result = _questBypasses.FirstOrDefault(x => x.Scene == scene && x.Item == item)) != null;
    }
}
