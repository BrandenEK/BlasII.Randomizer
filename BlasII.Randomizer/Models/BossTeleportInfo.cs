
namespace BlasII.Randomizer.Models;

/// <summary>
/// Information regarding a boss room teleport
/// </summary>
public class BossTeleportInfo(string exitScene, string entryScene, int entryDoor)
{
    /// <summary>
    /// The scene id of the boss room
    /// </summary>
    public string ExitScene { get; } = exitScene;
    /// <summary>
    /// The scene id of the room you teleport to
    /// </summary>
    public string EntryScene { get; } = entryScene;
    /// <summary>
    /// The entry id that you spawn at
    /// </summary>
    public int EntryDoor { get; } = entryDoor;
}
