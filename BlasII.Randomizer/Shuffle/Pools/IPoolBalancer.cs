using BlasII.Randomizer.Shuffle.Models;

namespace BlasII.Randomizer.Shuffle.Pools;

internal interface IPoolBalancer
{
    public void Balance(LocationPool progLocations, LocationPool junkLocations, ItemPool progItems, ItemPool junkItems);
}
