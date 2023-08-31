
namespace BlasII.Randomizer.Items
{
    public class Item
    {
        public readonly string id;

        public readonly string name;
        public readonly string hint;

        public readonly ItemType type;
        public readonly bool progression;
        public readonly int count;

        public enum ItemType
        {
            Rosary,
            Prayer,
            QuestItem,
            Ability,
            Weapon,
            Figure,
            Tears,
            Marks,
        }
    }
}