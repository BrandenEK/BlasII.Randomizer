
namespace BlasII.Randomizer.Shuffle.PoolCreators;

internal interface IPoolBalancer
{
    public void Balance(LocationPool progLocations, LocationPool junkLocations, ItemPool progItems, ItemPool junkItems);
}
