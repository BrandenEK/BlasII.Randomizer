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

        /// <summary>
        /// If this is a certain type of location, make sure the config allows it to be shuffled
        /// </summary>
        public bool ShouldBeShuffled(TempConfig config)
        {
            if (type == ItemLocationType.LongQuest && !config.shuffleLongQuests)
                return false;

            if (type == ItemLocationType.Shop && !config.shuffleShops)
                return false;

            return true;
        }

        public enum ItemLocationType
        {
            Normal,
            BossKey,
            LongQuest,
            Shop,
        }
    }
}