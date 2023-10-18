using BlasII.ModdingAPI.Storage;
using Il2CppTGK.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace BlasII.Randomizer.Items
{
    public class Item
    {
        // Static data

        [JsonProperty] public readonly string id;

        [JsonProperty] public readonly string name;
        [JsonProperty] public readonly string hint;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty] public readonly ItemType type;

        [JsonProperty] public readonly bool progression;
        [JsonProperty] public readonly int count;

        [JsonProperty] public readonly string[] subItems;
        [JsonProperty] public readonly bool removePrevious;

        // Calculated data

        public Sprite Image
        {
            get
            {
                if (IsProgressiveItem)
                    throw new System.Exception("Accessing a progressive item directly!");

                return type switch
                {
                    ItemType.RosaryBead => ItemStorage.TryGetRosaryBead(id, out var bead) ? bead.image : null,
                    ItemType.Prayer => ItemStorage.TryGetPrayer(id, out var prayer) ? prayer.image : null,
                    ItemType.Figurine => ItemStorage.TryGetFigure(id, out var figure) ? figure.image : null,
                    ItemType.QuestItem => ItemStorage.TryGetQuestItem(id, out var quest) ? quest.image : null,
                    ItemType.Weapon => id switch
                    {
                        "WE01" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Censer),
                        "WE02" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Blade),
                        "WE03" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Rapier),
                        _ => null,
                    },
                    ItemType.Ability => id switch
                    {
                        "AB44" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.WallClimb),
                        "AB02" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.DoubleJump),
                        "AB01" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.AirDash),
                        "AB35" => Main.Randomizer.Data.GetImage(DataStorage.ImageType.CherubRing),
                        _ => null,
                    },
                    ItemType.Cherub => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Cherub),
                    ItemType.Tears => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Tears),
                    ItemType.Marks => ItemStorage.TryGetQuestItem("QI99", out var marks) ? marks.image : null,

                    ItemType.Invalid => Main.Randomizer.Data.GetImage(DataStorage.ImageType.Invalid),
                    _ => null,
                };
            }
        }

        public string DisplayName
        {
            get
            {
                if (IsProgressiveItem)
                    throw new System.Exception("Accessing a progressive item directly!");

                return type switch
                {
                    ItemType.RosaryBead => ItemStorage.TryGetRosaryBead(id, out var bead) ? bead.caption : null,
                    ItemType.Prayer => ItemStorage.TryGetPrayer(id, out var prayer) ? prayer.caption : null,
                    ItemType.Figurine => ItemStorage.TryGetFigure(id, out var figure) ? figure.caption : null,
                    ItemType.QuestItem => ItemStorage.TryGetQuestItem(id, out var quest) ? quest.caption : null,
                    ItemType.Weapon => Main.Randomizer.LocalizationHandler.Localize(id switch
                    {
                        "WE01" => "we1n",
                        "WE02" => "we2n",
                        "WE03" => "we3n",
                        _ => null,
                    }),
                    ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize(id switch
                    {
                        "AB44" => "ab1n",
                        "AB02" => "ab2n",
                        "AB01" => "ab3n",
                        "AB35" => "ab4n",
                        _ => null,
                    }),
                    ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("chrn"),
                    ItemType.Tears => Main.Randomizer.LocalizationHandler.Localize("tern"),
                    ItemType.Marks => Main.Randomizer.LocalizationHandler.Localize("marn"),

                    ItemType.Invalid => "You should not see this.",
                    _ => null,
                };
            }
        }

        public string Description
        {
            get
            {
                if (IsProgressiveItem)
                    throw new System.Exception("Accessing a progressive item directly!");

                return type switch
                {
                    ItemType.RosaryBead => ItemStorage.TryGetRosaryBead(id, out var bead) ? bead.description : null,
                    ItemType.Prayer => ItemStorage.TryGetPrayer(id, out var prayer) ? prayer.description : null,
                    ItemType.Figurine => ItemStorage.TryGetFigure(id, out var figure) ? figure.description : null,
                    ItemType.QuestItem => ItemStorage.TryGetQuestItem(id, out var quest) ? quest.description : null,
                    ItemType.Weapon => Main.Randomizer.LocalizationHandler.Localize(id switch
                    {
                        "WE01" => "we1d",
                        "WE02" => "we2d",
                        "WE03" => "we3d",
                        _ => null,
                    }),
                    ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize(id switch
                    {
                        "AB44" => "ab1d",
                        "AB02" => "ab2d",
                        "AB01" => "ab3d",
                        "AB35" => "ab4d",
                        _ => null,
                    }),
                    ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("chrd"),
                    ItemType.Tears => Main.Randomizer.LocalizationHandler.Localize("terd"),
                    ItemType.Marks => Main.Randomizer.LocalizationHandler.Localize("mard"),

                    ItemType.Invalid => "You should not see this.",
                    _ => null,
                };
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

        // Obtaining item stuff

        public void GiveReward()
        {
            if (IsProgressiveItem)
                throw new System.Exception("Accessing a progressive item directly!");

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
                            {
                                // Upgrade the weapon
                                CoreCache.WeaponMemoryManager.UpgradeWeaponTier(weapon);
                            }
                            else
                            {
                                // Unlock the weapon and give the switching ability
                                CoreCache.EquipmentManager.Unlock(weapon);
                                if (AbilityStorage.TryGetAbility("AB10", out var ability))
                                    CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                            }
                        }
                        break;
                    }
                case ItemType.Ability:
                    {
                        if (AbilityStorage.TryGetAbility(id, out var ability))
                            CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                        break;
                    }
                case ItemType.Cherub:
                    {
                        QuestManager_SetQuestInt_Patch.CherubQuestFlag = true;
                        int currentCherubs = Main.Randomizer.GetQuestInt("ST16", "FREED_CHERUBS");
                        Main.Randomizer.SetQuestValue("ST16", "FREED_CHERUBS", currentCherubs + 1);
                        QuestManager_SetQuestInt_Patch.CherubQuestFlag = false;
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

            Main.Randomizer.ItemHandler.SetItemCollected(id);
        }

        // Progressive item stuff

        private bool IsProgressiveItem => subItems != null;

        public Item Current => IsProgressiveItem ? CurrentSubItem : this;
        public Item Upgraded => IsProgressiveItem ? UpgradedSubItem : this;

        public void RemovePreviousItem()
        {
            if (!IsProgressiveItem || !removePrevious)
                return;

            Item currentItem = CurrentSubItem;
            if (currentItem == null)
                return;

            // Only used for quest items right now
            if (ItemStorage.TryGetQuestItem(currentItem.id, out var item))
            {
                Main.Randomizer.Log($"Removing previous subitem for {id}: {currentItem.id}");
                ItemStorage.PlayerInventory.RemoveItem(item);
            }
        }

        private int GetSubItemLevel(bool upgraded)
        {
            for (int i = 0; i < subItems.Length; i++)
            {
                if (!Main.Randomizer.ItemHandler.IsItemCollected(subItems[i]))
                {
                    return i - (upgraded ? 0 : 1);
                }
            }

            return subItems.Length - (upgraded ? 0 : 1);
        }

        private Item CurrentSubItem
        {
            get
            {
                int currentLevel = GetSubItemLevel(false);

                if (currentLevel < 0)
                {
                    Main.Randomizer.LogError("Trying to access current subitem that hasn't been collected yet!");
                    return null;
                }

                Main.Randomizer.Log($"Getting current subitem for {id}: {subItems[currentLevel]}");
                return Main.Randomizer.Data.GetItem(subItems[currentLevel]);
            }
        }

        private Item UpgradedSubItem
        {
            get
            {
                int upgradedLevel = GetSubItemLevel(true);

                if (upgradedLevel >= subItems.Length)
                {
                    Main.Randomizer.LogError("Trying to access upgraded subitem that is already fully collected!");
                    return null;
                }

                Main.Randomizer.Log($"Getting upgraded subitem for {id}: {subItems[upgradedLevel]}");
                return Main.Randomizer.Data.GetItem(subItems[upgradedLevel]);
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
            Cherub = 6,
            Tears = 20,
            Marks = 21,
            Invalid = 99,
        }
    }
}