
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models a transition between two scenes
/// </summary>
public class Door : IUnique
{
    /// <summary>
    /// This door's identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// This door's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Flags that determine how this door will be shuffled
    /// </summary>
    public string Flags { get; set; }

    /// <summary>
    /// TEMP
    /// </summary>
    public DoorType Type { get; set; }

    /// <summary>
    /// Determines how this door will be matched to others
    /// </summary>
    public DoorDirection Direction { get; set; }

    /// <summary>
    /// This door's vanilla connection
    /// </summary>
    public string Exit { get; set; }

    // TEMP: Change this to flags
    public enum DoorType
    {
        Vanilla,
        Normal,
        Zone,
        Dlc,
    }

    /// <summary>
    /// Possible directions for a door
    /// </summary>
    public enum DoorDirection
    {
        Left,
        Right,
        Down,
        Up,
        DoorIn,
        DoorOut,
        ElevatorUp,
        ElevatorDown,
        MirrorUp,
        MirrorDown,
        BirdLeft,
        BirdRight,
    }
}
