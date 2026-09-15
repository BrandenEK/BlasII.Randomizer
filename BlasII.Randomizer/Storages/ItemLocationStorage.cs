using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class ItemLocationStorage : BaseInfoStorage<ItemLocation>
{
    /// <inheritdoc/>
    public ItemLocationStorage() : base("itemlocations.json", "item locations") { }
}
