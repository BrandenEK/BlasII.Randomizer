
namespace BlasII.Randomizer.Doors
{
    public class DoorLocation
    {
        public readonly string id;

        public readonly string room;
        public readonly DoorLocationType type;
        public readonly string originalDoor;
        public readonly Direction direction;

        public readonly string logic;

        public enum DoorLocationType
        {
            Normal,
            ZoneTransition,
            Vanilla,
        }

        public enum Direction
        {
            Up,
            Left,
            Right,
            Down,
            DoorEnter,
            DoorExit,
        }
    }
}