using BlasII.Randomizer.Items;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
using Il2CppTGK.Game.Components.StatsSystem.Data;
using Il2CppTGK.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer
{
    public class DataStorage
    {
        private bool _loaded = false;

        // Items
        private readonly Dictionary<string, Item> _allItems = new();

        public Item GetItem(string id) => _allItems.TryGetValue(id, out var item) ? item : null;
        public IEnumerable<Item> GetAllItems() => _allItems.Values;

        // Item locations
        private readonly Dictionary<string, ItemLocation> _allItemLocations = new();

        public ItemLocation GetItemLocation(string id) => _allItemLocations.TryGetValue(id, out var itemLocation) ? itemLocation : null;
        public IEnumerable<ItemLocation> GetAllItemLocations() => _allItemLocations.Values;

        // Rosary beads
        private readonly Dictionary<string, RosaryBeadItemID> _rosaryBeads = new();

        public bool TryGetRosaryBead(string id, out RosaryBeadItemID bead) => _rosaryBeads.TryGetValue(id, out bead);
        public IEnumerable<RosaryBeadItemID> GetAllRosaryBeads() => _rosaryBeads.Values;

        // Prayers
        private readonly Dictionary<string, PrayerItemID> _prayers = new();

        public bool TryGetPrayer(string id, out PrayerItemID prayer) => _prayers.TryGetValue(id, out prayer);
        public IEnumerable<PrayerItemID> GetAllPrayers() => _prayers.Values;

        // Figurines
        private readonly Dictionary<string, FigureItemID> _figurines = new();

        public bool TryGetFigurine(string id, out FigureItemID figurine) => _figurines.TryGetValue(id, out figurine);
        public IEnumerable<FigureItemID> GetAllFigurines() => _figurines.Values;

        // Quest items
        private readonly Dictionary<string, QuestItemID> _questItems = new();

        public bool TryGetQuestItem(string id, out QuestItemID questItem) => _questItems.TryGetValue(id, out questItem);
        public IEnumerable<QuestItemID> GetAllQuestItems() => _questItems.Values;

        // Abilities
        private readonly Dictionary<string, IAbilityTypeRef> _abilities = new();

        public bool TryGetAbility(string id, out IAbilityTypeRef ability) => _abilities.TryGetValue(id, out ability);
        public IEnumerable<IAbilityTypeRef> GetAllAbilities() => _abilities.Values;

        // Weapons
        private readonly Dictionary<string, WeaponID> _weapons = new();

        public bool TryGetWeapon(string id, out WeaponID weapon) => _weapons.TryGetValue(id, out weapon);
        public IEnumerable<WeaponID> GetAllWeapons() => _weapons.Values;

        // Armors
        private readonly Dictionary<string, ArmorID> _armors = new();

        // Value stats
        private readonly Dictionary<string, ValueStatID> _valueStats = new();

        public bool TryGetValueStat(string id, out ValueStatID valueStat) => _valueStats.TryGetValue(id, out valueStat);
        
        // Range stats
        private readonly Dictionary<string, RangeStatID> _rangeStats = new();

        public bool TryGetRangeStat(string id, out RangeStatID rangeStat) => _rangeStats.TryGetValue(id, out rangeStat);

        // Loading

        public void LoadAllObjects()
        {
            if (_loaded)
                return;

            // Items
            LoadObjectsOfType(_rosaryBeads);
            LoadObjectsOfType(_prayers);
            LoadObjectsOfType(_figurines);
            LoadObjectsOfType(_questItems);

            // Abilities
            LoadObjectsOfType(_abilities);

            // Stats
            LoadObjectsOfType(_valueStats);
            LoadObjectsOfType(_rangeStats);

            // Weapons
            LoadObjectsOfType(_weapons);
            LoadObjectsOfType(_armors);

            _loaded = true;
        }

        private void LoadObjectsOfType<T>(Dictionary<string, T> storage) where T : ScriptableObject
        {
            storage.Clear();
            foreach (T obj in Resources.FindObjectsOfTypeAll<T>())
            {
                storage.Add(obj.name, obj);
            }
        }
    }
}
