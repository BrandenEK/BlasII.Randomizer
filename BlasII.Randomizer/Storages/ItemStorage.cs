using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class ItemStorage : BaseInfoStorage<Item>
{
    /// <inheritdoc/>
    public ItemStorage() : base("items.json", "items") { }

    /// <summary>
    /// Represents a missing item
    /// </summary>
    public Item InvalidItem => _values["INVALID"];
}
