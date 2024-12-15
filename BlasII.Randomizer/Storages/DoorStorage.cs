using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class DoorStorage : BaseInfoStorage<Item>
{
    /// <inheritdoc/>
    public DoorStorage() : base("doors.json", "doors") { }
}
