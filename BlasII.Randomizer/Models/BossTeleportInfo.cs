
namespace BlasII.Randomizer.Models;

/// <summary>
/// Information regarding a boss room teleport
/// </summary>
public class BossTeleportInfo(string exitScene, int exitSceneHash, string entryScene, int entryDoor, bool forceDeactivate)
{
    /// <summary>
    /// The scene id of the boss room
    /// </summary>
    public string ExitScene { get; } = exitScene;
    /// <summary>
    /// The hash code of the boss room
    /// </summary>
    public int ExitSceneHash { get; } = exitSceneHash;
    /// <summary>
    /// The scene id of the room you teleport to
    /// </summary>
    public string EntryScene { get; } = entryScene;
    /// <summary>
    /// The entry id that you spawn at
    /// </summary>
    public int EntryDoor { get; } = entryDoor;
    /// <summary>
    /// Should the room be force reloaded after teleport
    /// </summary>
    public bool ForceDeactivate { get; } = forceDeactivate;
}
