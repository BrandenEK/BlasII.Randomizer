using System.Collections.Generic;

namespace BlasII.Randomizer.Shops;

/// <summary>
/// Provides functionality for one of the shops
/// </summary>
public interface IShop
{
    /// <summary>
    /// Returns a list of the item costs in the shop
    /// </summary>
    public IEnumerable<int> GetVanillaCosts();
}

internal class HandShop : IShop
{
    private readonly int[] _costs = [3000, 3000, 3000, 3000, 3000, 3000, 3000, 6000, 12000, 12000, 17500, 32000];

    public IEnumerable<int> GetVanillaCosts()
    {
        return _costs;
    }
}

internal class PatioShop : IShop
{
    private readonly int[] _costs = [6000, 6000, 6000, 6000, 12000, 12000, 12000];

    public IEnumerable<int> GetVanillaCosts()
    {
        return _costs;
    }
}

internal class TravelerShop : IShop
{
    public IEnumerable<int> GetVanillaCosts()
    {
        yield return 3000;
        yield return 3000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z09_VISITED")) yield return 6000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z05_VISITED")) yield return 6000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z11_VISITED")) yield return 6000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z12_VISITED")) yield return 6000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z01_VISITED")) yield return 12000;
        if (Main.Randomizer.GetQuestBool("ST06", "Z10_VISITED")) yield return 17500;
        // TODO: add item from dlc quest
    }
}

internal class FakeTravelerShop : IShop
{
    public IEnumerable<int> GetVanillaCosts()
    {
        yield return 3000;
        yield return 3000;
        if (TEMP_VALUE >= 0) yield return 6000;
        if (TEMP_VALUE >= 1) yield return 6000;
        if (TEMP_VALUE >= 2) yield return 6000;
        if (TEMP_VALUE >= 3) yield return 6000;
        if (TEMP_VALUE >= 4) yield return 12000;
        if (TEMP_VALUE >= 5) yield return 17500;
    }

    private const int TEMP_VALUE = 5;
}
