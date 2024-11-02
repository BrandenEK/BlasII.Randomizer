using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace BlasII.Randomizer.Items;

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
                ItemType.RosaryBead => AssetStorage.Beads[id].image,
                ItemType.Prayer => AssetStorage.Prayers[id].image,
                ItemType.Figurine => AssetStorage.Figures[id].image,
                ItemType.QuestItem => AssetStorage.QuestItems[id].image,
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
                ItemType.Marks => AssetStorage.QuestItems["QI99"].image,

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
                ItemType.RosaryBead => AssetStorage.Beads[id].caption,
                ItemType.Prayer => AssetStorage.Prayers[id].caption,
                ItemType.Figurine => AssetStorage.Figures[id].caption,
                ItemType.QuestItem => AssetStorage.QuestItems[id].caption,
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
                ItemType.Tears => Amount + " " + Main.Randomizer.LocalizationHandler.Localize("tern"),
                ItemType.Marks => Amount + " " + Main.Randomizer.LocalizationHandler.Localize("marn"),

                ItemType.Invalid => "Invalid Item",
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
                ItemType.RosaryBead => AssetStorage.Beads[id].description,
                ItemType.Prayer => AssetStorage.Prayers[id].description,
                ItemType.Figurine => AssetStorage.Figures[id].description,
                ItemType.QuestItem => AssetStorage.QuestItems[id].description,
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
                    if (AssetStorage.Beads.TryGetAsset(id, out var bead))
                        AssetStorage.PlayerInventory.AddItemAsync(bead, 0, true);
                    break;
                }
            case ItemType.Prayer:
                {
                    if (AssetStorage.Prayers.TryGetAsset(id, out var prayer))
                        AssetStorage.PlayerInventory.AddItemAsync(prayer, 0, true);
                    break;
                }
            case ItemType.Figurine:
                {
                    if (AssetStorage.Figures.TryGetAsset(id, out var figure))
                        AssetStorage.PlayerInventory.AddItemAsync(figure, 0, true);
                    break;
                }
            case ItemType.QuestItem:
                {
                    if (AssetStorage.QuestItems.TryGetAsset(id, out var quest))
                        AssetStorage.PlayerInventory.AddItemAsync(quest, 0, true);
                    break;
                }
            case ItemType.Weapon:
                {
                    var weapon = AssetStorage.Weapons[id];

                    if (CoreCache.EquipmentManager.IsUnlocked(weapon))
                    {
                        // Upgrade the weapon
                        CoreCache.WeaponMemoryManager.UpgradeWeaponTier(weapon);
                    }
                    else
                    {
                        // Unlock the weapon and give the switching ability
                        var ability = AssetStorage.Abilities["AB10"];
                        CoreCache.EquipmentManager.Unlock(weapon);
                        CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    }
                    break;
                }
            case ItemType.Ability:
                {
                    var ability = AssetStorage.Abilities[id];
                    CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    break;
                }
            case ItemType.Cherub:
                {
                    int currentCherubs = Main.Randomizer.GetQuestInt("ST16", "FREED_CHERUBS");
                    Main.Randomizer.SetQuestValue("ST16", "FREED_CHERUBS", currentCherubs + 1);
                    break;
                }
            case ItemType.Tears:
                {
                    AssetStorage.PlayerStats.AddRewardTears(Amount);
                    break;
                }
            case ItemType.Marks:
                {
                    AssetStorage.PlayerStats.AddRewardOrbs(Amount, true);
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
        ModLog.Info($"Removing previous subitem for {id}: {currentItem.id}");
        var item = AssetStorage.QuestItems[currentItem.id];
        AssetStorage.PlayerInventory.RemoveItem(item);
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
                ModLog.Error("Trying to access current subitem that hasn't been collected yet!");
                return null;
            }

            ModLog.Info($"Getting current subitem for {id}: {subItems[currentLevel]}");
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
                ModLog.Error("Trying to access upgraded subitem that is already fully collected!");
                return null;
            }

            ModLog.Info($"Getting upgraded subitem for {id}: {subItems[upgradedLevel]}");
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