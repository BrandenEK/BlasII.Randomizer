
namespace BlasII.Randomizer.Models;

/// <summary>
/// Handles the data for progressive quest items
/// </summary>
public class ProgressiveItemData(string id, params ProgressiveItemLevel[] levels)
{
    /// <summary>
    /// The ID for this item
    /// </summary>
    public string Id { get; } = id;

    private readonly ProgressiveItemLevel[] _levels = levels;

    /// <summary>
    /// The data for the current level of the item
    /// </summary>
    public ProgressiveItemLevel CurrentProgressiveLevel
    {
        get
        {
            int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id) - 1;
            return level >= 0 && level < _levels.Length ? _levels[level] : new ProgressiveItemLevel("INVALID");
        }
    }

    /// <summary>
    /// The data for the next level of the item
    /// </summary>
    public ProgressiveItemLevel NextProgressiveLevel
    {
        get
        {
            int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id);
            return level >= 0 && level < _levels.Length ? _levels[level] : new ProgressiveItemLevel("INVALID");
        }
    }

    /// <summary>
    /// Attempts to get the data for the next level of the item
    /// </summary>
    public bool TryGetNextProgressiveLevel(out ProgressiveItemLevel nextLevel)
    {
        int level = Main.Randomizer.ItemHandler.AmountItemCollected(Id);

        if (level >= 0 && level < _levels.Length)
        {
            nextLevel = _levels[level];
            return true;
        }

        nextLevel = new ProgressiveItemLevel("INVALID");
        return false;
    }
}
