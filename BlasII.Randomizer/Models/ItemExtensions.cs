using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Shops;
using BlasII.Randomizer.Storages;
using Il2CppTGK.Game;
using System;
using UnityEngine;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for the Item model
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Ensures the item is not of type INVALID
    /// </summary>
    public static bool IsValid(this Item item)
    {
        return item.Type != Item.ItemType.Invalid;
    }

    /// <summary>
    /// Retrieves the sprite for this item
    /// </summary>
    public static Sprite GetSprite(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].image,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].image,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].image,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].image,
            Item.ItemType.ProgressiveQuestItem => item.GetProgressiveItem(true).GetSprite(),
            Item.ItemType.GoldLump => AssetStorage.QuestItems["QI105"].image,
            Item.ItemType.Weapon => Main.Randomizer.EmbeddedIconStorage.GetImage(item.Id),
            Item.ItemType.Ability => Main.Randomizer.EmbeddedIconStorage.GetImage(item.Id),
            Item.ItemType.Cherub => Main.Randomizer.EmbeddedIconStorage.GetImage("Cherub"),
            Item.ItemType.Tears => Main.Randomizer.EmbeddedIconStorage.GetImage("Tears"),
            Item.ItemType.Marks => Main.Randomizer.EmbeddedIconStorage.GetImage("Marks"),
            Item.ItemType.PreMarks => Main.Randomizer.EmbeddedIconStorage.GetImage("PreMarks"),

            Item.ItemType.Invalid => Main.Randomizer.CustomIconStorage.GetImage(Storages.CustomIconStorage.IconType.Invalid),
            _ => throw new Exception($"Invalid item type: {item.Type}")
        };
    }

    /// <summary>
    /// Retrieves the display name for this item
    /// </summary>
    public static string GetName(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].caption,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].caption,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].caption,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].caption,
            Item.ItemType.ProgressiveQuestItem => item.GetProgressiveItem(true).GetName(),
            Item.ItemType.GoldLump => AssetStorage.QuestItems["QI105"].caption,
            Item.ItemType.Weapon => item.IsWeaponUnlocked()
                ? Main.Randomizer.LocalizationHandler.Localize("{0} {1}", $"weapon/{item.Id.ToLower()}/name", "weapon/upgrade/name")
                : Main.Randomizer.LocalizationHandler.Localize($"weapon/{item.Id.ToLower()}/name"),
            Item.ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize($"ability/{item.Id.ToLower()}/name"),
            Item.ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("currency/cherub/name"),
            Item.ItemType.Tears => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("currency/tears/name"),
            Item.ItemType.Marks => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("currency/marks/name"),
            Item.ItemType.PreMarks => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("currency/premarks/name"),

            Item.ItemType.Invalid => "Invalid Item",
            _ => throw new Exception($"Invalid item type: {item.Type}")
        };
    }

    /// <summary>
    /// Retrieves the description of this item
    /// </summary>
    public static string GetDescription(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].description,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].description,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].description,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].description,
            Item.ItemType.ProgressiveQuestItem => item.GetProgressiveItem(true).GetDescription(),
            Item.ItemType.GoldLump => AssetStorage.QuestItems["QI105"].description,
            Item.ItemType.Weapon => item.IsWeaponUnlocked()
                ? Main.Randomizer.LocalizationHandler.Localize("{0} {1}", "weapon/upgrade/desc", $"weapon/{item.Id.ToLower()}/name")
                : Main.Randomizer.LocalizationHandler.Localize($"weapon/{item.Id.ToLower()}/desc"),
            Item.ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize($"ability/{item.Id.ToLower()}/desc"),
            Item.ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("currency/cherub/desc"),
            Item.ItemType.Tears => Main.Randomizer.LocalizationHandler.Localize("currency/tears/desc"),
            Item.ItemType.Marks => Main.Randomizer.LocalizationHandler.Localize("currency/marks/desc"),
            Item.ItemType.PreMarks => Main.Randomizer.LocalizationHandler.Localize("currency/premarks/desc"),

            Item.ItemType.Invalid => "You should not see this.",
            _ => throw new Exception($"Invalid item type: {item.Type}")
        };
    }

    /// <summary>
    /// Retrieves the value of this item
    /// </summary>
    public static ShopValue GetValue(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead or Item.ItemType.Prayer or Item.ItemType.Figurine or Item.ItemType.QuestItem => item.Id switch
            {
                "QI63" or "QI64" or "QI65" or "QI66" or "QI67" => ShopValue.BossKeys,
                _ => item.Class switch
                {
                    Item.ItemClass.Filler => ShopValue.FillerInventory,
                    Item.ItemClass.Useful => ShopValue.UsefulInventory,
                    Item.ItemClass.Progression => ShopValue.ProgressionInventory,
                    _ => throw new Exception($"Invalid item class: {item.Class}")
                }
            },
            Item.ItemType.ProgressiveQuestItem => item.GetProgressiveItem(true).GetValue(),
            Item.ItemType.Cherub or Item.ItemType.GoldLump => ShopValue.Cherubs,
            Item.ItemType.Weapon or Item.ItemType.Ability => ShopValue.WeaponsAndAbilities,
            Item.ItemType.Tears => item.GetAmount() >= 2000 ? ShopValue.HighTears : ShopValue.LowTears,
            Item.ItemType.Marks => item.GetAmount() switch
            {
                1 => ShopValue.FillerInventory,
                2 or 3 => ShopValue.UsefulInventory,
                4 or 5 => ShopValue.ProgressionInventory,
                _ => throw new Exception($"Invalid item amount: {item.GetAmount()}")
            },
            Item.ItemType.PreMarks => ShopValue.ProgressionInventory,

            Item.ItemType.Invalid => ShopValue.FillerInventory,
            _ => throw new Exception($"Invalid item type: {item.Type}")
        };
    }

    /// <summary>
    /// Grants the item
    /// </summary>
    public static void GiveReward(this Item item)
    {
        switch (item.Type)
        {
            case Item.ItemType.RosaryBead:
                {
                    if (AssetStorage.Beads.TryGetValue(item.Id, out var bead))
                        AssetStorage.PlayerInventory.AddItemAsync(bead, 0, true);
                    break;
                }
            case Item.ItemType.Prayer:
                {
                    if (AssetStorage.Prayers.TryGetValue(item.Id, out var prayer))
                        AssetStorage.PlayerInventory.AddItemAsync(prayer, 0, true);
                    break;
                }
            case Item.ItemType.Figurine:
                {
                    if (AssetStorage.Figures.TryGetValue(item.Id, out var figure))
                        AssetStorage.PlayerInventory.AddItemAsync(figure, 0, true);
                    break;
                }
            case Item.ItemType.QuestItem:
                {
                    if (AssetStorage.QuestItems.TryGetValue(item.Id, out var quest))
                        AssetStorage.PlayerInventory.AddItemAsync(quest, 0, true);
                    break;
                }
            case Item.ItemType.ProgressiveQuestItem:
                {
                    var nextItem = item.GetProgressiveItem(true);
                    if (!nextItem.IsValid())
                    {
                        ModLog.Error($"Attempting to add invalid level of item {item.Id}");
                        break;
                    }

                    // Add next item if there is one
                    AssetStorage.PlayerInventory.AddItemAsync(AssetStorage.QuestItems[nextItem.Id]);
                    if (nextItem.Id == "QI111")
                        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ABILITY_IDS.GoldFlask], true);

                    var currentItem = item.GetProgressiveItem(false);
                    if (!currentItem.IsValid() || item.Id == "ST")
                        break;

                    // Remove current item if you have one
                    AssetStorage.PlayerInventory.RemoveItem(AssetStorage.QuestItems[currentItem.Id]);
                    break;
                }
            case Item.ItemType.GoldLump:
                {
                    var goldItem = AssetStorage.QuestItems["QI105"];
                    int amount = Main.Randomizer.ItemHandler.AmountItemCollected(item.Id) + 1;

                    AssetStorage.PlayerInventory.AddItemAsync(goldItem, 0, true);
                    Main.Randomizer.SetQuestValue("ST103", "GOLD_TAKEN_FREE", Math.Min(amount, 10));
                    Main.Randomizer.SetQuestValue("ST103", "GOLD_TAKEN_PAID", Math.Max(amount - 10, 0));
                    break;
                }
            case Item.ItemType.Weapon:
                {
                    var weapon = AssetStorage.Weapons[Enum.Parse<WEAPON_IDS>(item.Id)];
                    var ruego = AssetStorage.Weapons[WEAPON_IDS.RosaryBlade];
                    var meaculpa = AssetStorage.Weapons[WEAPON_IDS.MeaCulpa];

                    if (CoreCache.EquipmentManager.IsUnlocked(weapon))
                    {
                        // Upgrade the weapon
                        CoreCache.WeaponMemoryManager.UpgradeWeaponTier(weapon);
                        break;
                    }

                    // Handle special case for getting regular weapon last
                    if (CoreCache.EquipmentManager.CountUnlockedWeapons() == 3 &&
                        CoreCache.EquipmentManager.GetWeaponSlot(ruego) != -1 &&
                        CoreCache.EquipmentManager.GetWeaponSlot(meaculpa) != -1)
                    {
                        var swapWeapon = CoreCache.EquipmentManager.GetCurrentWeapon() == meaculpa ? ruego : meaculpa;

                        ModLog.Info($"Hiding selection for weapon: {swapWeapon.name}");
                        CoreCache.EquipmentManager.ForceSetWeaponToSlot(null, CoreCache.EquipmentManager.GetWeaponSlot(swapWeapon));
                    }

                    // Unlock the weapon and give the switching ability
                    var ability = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
                    CoreCache.EquipmentManager.Unlock(weapon);
                    CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    break;
                }
            case Item.ItemType.Ability:
                {
                    var ability = AssetStorage.Abilities[Enum.Parse<ABILITY_IDS>(item.Id)];
                    CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    break;
                }
            case Item.ItemType.Cherub:
                {
                    int amount = Main.Randomizer.ItemHandler.AmountItemCollected(item.Id) + 1;
                    Main.Randomizer.SetQuestValue("ST16", "FREED_CHERUBS", amount);
                    break;
                }
            case Item.ItemType.Tears:
                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["Tears"], item.GetAmount());
                    break;
                }
            case Item.ItemType.Marks:
                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["Orbs"], item.GetAmount());
                    break;
                }
            case Item.ItemType.PreMarks:

                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["MarksPreceptor"], item.GetAmount());
                    break;
                }
        }

        Main.Randomizer.ItemHandler.SetItemCollected(item.Id);
        Main.Randomizer.MessageHandler.Broadcast("ITEM", item.Id);
    }

    /// <summary>
    /// Retrieves the numeric value of this item
    /// </summary>
    private static int GetAmount(this Item item)
    {
        return int.Parse(item.Id[3..]);
    }

    /// <summary>
    /// Returns the current or upgraded item
    /// </summary>
    private static Item GetProgressiveItem(this Item item, bool upgraded)
    {
        string[] itemIds = item.Id switch
        {
            "IL" => ItemGroups.Lacrimatorios,
            "ST" => ItemGroups.SculptorTools,
            "UL" => ItemGroups.Lullabies,
            _ => throw new Exception($"Invalid {Item.ItemType.ProgressiveQuestItem}: {item.Id}")
        };

        int level = Main.Randomizer.ItemHandler.AmountItemCollected(item.Id);
        if (!upgraded)
            level--;

        return level >= 0 && level < itemIds.Length
            ? Main.Randomizer.ItemStorage[itemIds[level]]
            : Main.Randomizer.ItemStorage.InvalidItem;
    }

    /// <summary>
    /// Retrieves the unlock status of this item
    /// </summary>
    private static bool IsWeaponUnlocked(this Item item)
    {
        var weapon = AssetStorage.Weapons[Enum.Parse<WEAPON_IDS>(item.Id)];
        return CoreCache.EquipmentManager.IsUnlocked(weapon);
    }
}
