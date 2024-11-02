using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlasII.Randomizer.Doors;

public class DoorLocation
{
    [JsonProperty] public readonly string id;

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty] public readonly DoorLocationType type;

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty] public readonly Direction direction;

    [JsonProperty] public readonly string room;
    [JsonProperty] public readonly string originalDoor;

    [JsonProperty] public readonly string logic;

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