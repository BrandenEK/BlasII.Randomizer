
namespace BlasII.Randomizer.Items
{
    public class ItemLocation
    {
        public readonly string id;

        public readonly string name;
        public readonly string hint;

        public readonly string room;
        public readonly ItemLocationType type;
        public readonly string originalItem;

        public readonly string logic;

        public enum ItemLocationType
        {
            Normal,
            Vanilla,
        }
    }
}