using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlasII.Randomizer.Items
{
    public class ItemLocation
    {
        [JsonProperty] public readonly string id;

        [JsonProperty] public readonly string name;
        [JsonProperty] public readonly string hint;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty] public readonly ItemLocationType type;

        [JsonProperty] public readonly string room;
        [JsonProperty] public readonly string originalItem;

        [JsonProperty] public readonly string logic;

        public enum ItemLocationType
        {
            Normal,
            Vanilla,
            BossKey,
        }
    }
}