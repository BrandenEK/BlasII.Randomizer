
namespace BlasII.Randomizer.Enemies
{
    public class EnemyLocation
    {
        public readonly string id;

        public EnemyLocationType type;
        public readonly string originalEnemy;

        public enum EnemyLocationType
        {
            Normal,
            Arena,
        }
    }
}