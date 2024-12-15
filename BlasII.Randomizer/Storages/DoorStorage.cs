using BlasII.Randomizer.Doors;
using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class DoorStorage : BaseInfoStorage<Door>
{
    /// <inheritdoc/>
    public DoorStorage() : base("doors.json", "doors") { }
}
