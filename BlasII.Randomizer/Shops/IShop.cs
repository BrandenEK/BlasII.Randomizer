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

    /// <summary>
    /// Returns a hardcoded unique number used to calculate random costs
    /// </summary>
    public int GetSeedValue();
}

internal class HandShop : IShop
{
    private readonly int[] _costs = [3000, 3000, 3000, 3000, 3000, 3000, 3000, 6000, 12000, 12000, 17500, 32000];
    //                    New costs: 3900, 3900, 3900, 3900, 3900, 3900, 3900, 7800, 15600, 15600, 23000, 42000

    public IEnumerable<int> GetVanillaCosts()
    {
        return _costs;
    }

    public int GetSeedValue()
    {
        return 1;
    }
}

internal class PatioShop : IShop
{
    private readonly int[] _costs = [6000, 6000, 6000, 6000, 12000, 12000, 12000];
    //                    New costs: 7800, 7800, ????, ????, 15600, 15600, ?????

    public IEnumerable<int> GetVanillaCosts()
    {
        return _costs;
    }

    public int GetSeedValue()
    {
        return 16;
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

        // New costs: 3000, 3500, 7800, 7800, 7800, 7800, 15600, 20000, 23000
    }

    public int GetSeedValue()
    {
        return 25;
    }
}
