using BlasII.ModdingAPI.Storage;
using Il2CppTGK.Game;
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
                        return ItemStorage.TryGetRosaryBead(id, out var bead) ? bead.image : null;
                    case ItemType.Prayer:
                        return ItemStorage.TryGetPrayer(id, out var prayer) ? prayer.image : null;
                    case ItemType.Figurine:
                        return ItemStorage.TryGetFigure(id, out var figure) ? figure.image : null;
                    case ItemType.QuestItem:
                        return ItemStorage.TryGetQuestItem(id, out var quest) ? quest.image : null;
                    case ItemType.Weapon:
                        return null;
                    case ItemType.Ability:
                        return null;
                    case ItemType.Tears:
                        return null;
                    case ItemType.Marks:
                        return ItemStorage.TryGetQuestItem("QI99", out var marks) ? marks.image : null;
                    default:
                        return null;
                }
            }
        }

        private int Amount
        {
            get
            {
                int leftBracket = id.IndexOf('['), rightBracket = id.IndexOf(']');
                return int.Parse(id.Substring(leftBracket + 1, rightBracket - leftBracket - 1));
            }
        }

        public void GiveReward()
        {
            switch (type)
            {
                case ItemType.RosaryBead:
                    {
                        if (ItemStorage.TryGetRosaryBead(id, out var bead))
                            ItemStorage.PlayerInventory.AddItemAsync(bead, 0, true);
                        break;
                    }
                case ItemType.Prayer:
                    {
                        if (ItemStorage.TryGetPrayer(id, out var prayer))
                            ItemStorage.PlayerInventory.AddItemAsync(prayer, 0, true);
                        break;
                    }
                case ItemType.Figurine:
                    {
                        if (ItemStorage.TryGetFigure(id, out var figure))
                            ItemStorage.PlayerInventory.AddItemAsync(figure, 0, true);
                        break;
                    }
                case ItemType.QuestItem:
                    {
                        if (ItemStorage.TryGetQuestItem(id, out var quest))
                            ItemStorage.PlayerInventory.AddItemAsync(quest, 0, true);
                        break;
                    }
                case ItemType.Weapon:
                    {
                        if (WeaponStorage.TryGetWeapon(id, out var weapon))
                        {
                            if (CoreCache.EquipmentManager.IsUnlocked(weapon))
                                CoreCache.WeaponMemoryManager.UpgradeWeaponTier(weapon);
                            else
                                CoreCache.EquipmentManager.Unlock(weapon);
                        }
                        break;
                    }
                case ItemType.Ability:
                    {
                        if (AbilityStorage.TryGetAbility(id, out var ability))
                            CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                        break;
                    }
                case ItemType.Tears:
                    {
                        StatStorage.PlayerStats.AddRewardTears(Amount);
                        break;
                    }
                case ItemType.Marks:
                    {
                        StatStorage.PlayerStats.AddRewardOrbs(Amount, true);
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
            Weapon = 4,
            Ability = 5,
            Tears = 20,
            Marks = 21,
        }
    }
}