
namespace BlasII.Randomizer.Items
{
    public class ItemHandler
    {
        public Item GetItemAtLocation(string locationId)
        {
            return null;
        }

        public void GiveItemAtLocation(string locationId)
        {
            Main.Randomizer.LogWarning("Giving location: " +  locationId);
        }
    }
}