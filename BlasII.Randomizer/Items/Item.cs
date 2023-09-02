using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace BlasII.Randomizer.Items
{
    public class Item
    {
        [JsonProperty] public readonly string id;

        [JsonProperty] public readonly string name;
        [JsonProperty] public readonly string hint;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty] public readonly ItemType type;

        [JsonProperty] public readonly bool progression;
        [JsonProperty] public readonly int count;

        public Sprite Image
        {
            get
            {
                switch (type)
                {
                    case ItemType.RosaryBead:
                        return Main.Randomizer.Data.TryGetRosaryBead(id, out var bead) ? bead.image : null;
                    case ItemType.Prayer:
                        return Main.Randomizer.Data.TryGetPrayer(id, out var prayer) ? prayer.image : null;
                    case ItemType.Figurine:
                        return Main.Randomizer.Data.TryGetFigurine(id, out var figure) ? figure.image : null;
                    case ItemType.QuestItem:
                        return Main.Randomizer.Data.TryGetQuestItem(id, out var quest) ? quest.image : null;
                    default:
                        return null;
                }
            }
        }

        public void GiveReward()
        {
            switch (type)
            {
                case ItemType.RosaryBead:
                    {
                        if (Main.Randomizer.Data.TryGetRosaryBead(id, out var bead))
                            Main.Randomizer.PlayerInventory.AddItemAsync(bead, 0, true);
                        break;
                    }
                case ItemType.Prayer:
                    {
                        if (Main.Randomizer.Data.TryGetPrayer(id, out var prayer))
                            Main.Randomizer.PlayerInventory.AddItemAsync(prayer, 0, true);
                        break;
                    }
                case ItemType.Figurine:
                    {
                        if (Main.Randomizer.Data.TryGetFigurine(id, out var figure))
                            Main.Randomizer.PlayerInventory.AddItemAsync(figure, 0, true);
                        break;
                    }
                case ItemType.QuestItem:
                {
                    if (Main.Randomizer.Data.TryGetQuestItem(id, out var quest))
                        Main.Randomizer.PlayerInventory.AddItemAsync(quest, 0, true);
                    break;
                }
            }
        }

        public enum ItemType
        {
            RosaryBead = 0,
            Prayer = 1,
            Figurine = 2,
            QuestItem = 3,
            Ability,
            Weapon,
            Tears,
            Marks,
        }
    }
}