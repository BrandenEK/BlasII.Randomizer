
namespace BlasII.Randomizer.Enemies
{
    public class Enemy
    {
        public readonly string id;

        public readonly EnemyType type;

        public enum EnemyType
        {
            Normal,
            Flying,
            Large,
            Vanilla,
        }
    }
}