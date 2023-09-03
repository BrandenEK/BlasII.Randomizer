using BlasII.Randomizer.Items;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
using Il2CppTGK.Game.Components.StatsSystem.Data;
using Il2CppTGK.Game.WeaponMemory;
using Il2CppTGK.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer
{
    public class DataStorage
    {
        // Items
        private readonly Dictionary<string, Item> _allItems = new();

        public Item GetItem(string id) => _allItems.TryGetValue(id, out var item) ? item : null;
        public bool DoesItemExist(string id) => _allItems.ContainsKey(id);
        public IEnumerable<Item> GetAllItems() => _allItems.Values;

        // Item locations
        private readonly Dictionary<string, ItemLocation> _allItemLocations = new();

        public ItemLocation GetItemLocation(string id) => _allItemLocations.TryGetValue(id, out var itemLocation) ? itemLocation : null;
        public bool DoesItemLocationExist(string id) => _allItemLocations.ContainsKey(id);
        public IEnumerable<ItemLocation> GetAllItemLocations() => _allItemLocations.Values;

        // Rosary beads
        private readonly Dictionary<string, RosaryBeadItemID> _rosaryBeads = new();

        public bool TryGetRosaryBead(string id, out RosaryBeadItemID bead) => _rosaryBeads.TryGetValue(id, out bead);
        public IEnumerable<RosaryBeadItemID> GetAllRosaryBeads() => _rosaryBeads.OrderBy(x => x.Key).Select(x => x.Value);

        // Prayers
        private readonly Dictionary<string, PrayerItemID> _prayers = new();

        public bool TryGetPrayer(string id, out PrayerItemID prayer) => _prayers.TryGetValue(id, out prayer);
        public IEnumerable<PrayerItemID> GetAllPrayers() => _prayers.OrderBy(x => x.Key).Select(x => x.Value);

        // Figurines
        private readonly Dictionary<string, FigureItemID> _figurines = new();

        public bool TryGetFigurine(string id, out FigureItemID figurine) => _figurines.TryGetValue(id, out figurine);
        public IEnumerable<FigureItemID> GetAllFigurines() => _figurines.OrderBy(x => x.Key).Select(x => x.Value);

        // Quest items
        private readonly Dictionary<string, QuestItemID> _questItems = new();

        public bool TryGetQuestItem(string id, out QuestItemID questItem) => _questItems.TryGetValue(id, out questItem);
        public IEnumerable<QuestItemID> GetAllQuestItems() => _questItems.OrderBy(x => x.Key).Select(x => x.Value);

        // Abilities
        private readonly Dictionary<string, IAbilityTypeRef> _abilities = new();

        public bool TryGetAbility(string id, out IAbilityTypeRef ability) => _abilities.TryGetValue(id, out ability);
        public IEnumerable<IAbilityTypeRef> GetAllAbilities() => _abilities.OrderBy(x => x.Key).Select(x => x.Value);

        // Weapons
        private readonly Dictionary<string, WeaponID> _weapons = new();

        public bool TryGetWeapon(string id, out WeaponID weapon) => _weapons.TryGetValue(id, out weapon);
        public IEnumerable<WeaponID> GetAllWeapons() => _weapons.OrderBy(x => x.Key).Select(x => x.Value);

        // Weapon memories
        private readonly Dictionary<string, WeaponMemoryID> _weaponMemories = new();

        public bool TryGetWeaponMemory(string id, out WeaponMemoryID weaponMemory) => _weaponMemories.TryGetValue(id, out weaponMemory);
        public IEnumerable<WeaponMemoryID> GetAllWeaponMemories() => _weaponMemories.OrderBy(x => x.Key).Select(x => x.Value);

        // Armors
        private readonly Dictionary<string, ArmorID> _armors = new();

        // Value stats
        private readonly Dictionary<string, ValueStatID> _valueStats = new();

        public bool TryGetValueStat(string id, out ValueStatID valueStat) => _valueStats.TryGetValue(id, out valueStat);
        
        // Range stats
        private readonly Dictionary<string, RangeStatID> _rangeStats = new();

        public bool TryGetRangeStat(string id, out RangeStatID rangeStat) => _rangeStats.TryGetValue(id, out rangeStat);

        // Loading

        public void Initialize()
        {
            LoadAllJsonData();
            LoadAllObjectData();
        }

        private void LoadAllJsonData()
        {
            if (Main.Randomizer.FileHandler.LoadDataAsJson("items.json", out Item[] items))
            {
                foreach (var item in items)
                    _allItems.Add(item.id, item);
            }
            Main.Randomizer.Log($"Loaded {_allItems.Count} items!");

            if (Main.Randomizer.FileHandler.LoadDataAsJson("item-locations.json", out ItemLocation[] itemLocations))
            {
                foreach (var itemLocation in itemLocations)
                    _allItemLocations.Add(itemLocation.id, itemLocation);
            }
            Main.Randomizer.Log($"Loaded {_allItemLocations.Count} item locations!");
        }

        private void LoadAllObjectData()
        {
            // Items
            LoadObjectsOfType(_rosaryBeads);
            LoadObjectsOfType(_prayers);
            LoadObjectsOfType(_figurines);
            LoadObjectsOfType(_questItems);

            // Abilities
            LoadObjectsOfType(_abilities, _abilityLookup);

            // Stats
            LoadObjectsOfType(_valueStats);
            LoadObjectsOfType(_rangeStats);

            // Weapons
            LoadObjectsOfType(_weapons, _weaponLookup);
            LoadObjectsOfType(_weaponMemories);
            LoadObjectsOfType(_armors);
        }

        private void LoadObjectsOfType<T>(Dictionary<string, T> storage) where T : ScriptableObject
        {
            foreach (T obj in Resources.FindObjectsOfTypeAll<T>())
            {
                storage.Add(obj.name, obj);
            }
        }

        private void LoadObjectsOfType<T>(Dictionary<string, T> storage, Dictionary<string, string> lookup) where T : ScriptableObject
        {
            foreach (T obj in Resources.FindObjectsOfTypeAll<T>())
            {
                if (lookup.TryGetValue(obj.name, out string id))
                    storage.Add(id, obj);
            }
        }

        private readonly Dictionary<string, string> _weaponLookup = new()
        {
            { "Censer", "WE01" },
            { "Rosary Blade", "WE02" },
            { "Rapier", "WE03" },
        };

        private readonly Dictionary<string, string> _abilityLookup = new()
        {
            { "Wall Climb Ability Type Ref", "AB01" },
            { "Air Jump Type Ref", "AB02" },
            { "Air Dash Type Ref", "AB03" },
            { "Magic Ring Climb Type Ref", "AB04" },
        };
    }
}
