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

        private readonly Dictionary<string, RosaryBeadItemID> _rosaryBeads = new();
        private readonly Dictionary<string, PrayerItemID> _prayers = new();
        private readonly Dictionary<string, FigureItemID> _figurines = new();
        private readonly Dictionary<string, QuestItemID> _questItems = new();

        private readonly Dictionary<string, IAbilityTypeRef> _abilities = new();

        private readonly Dictionary<string, ValueStatID> _valueStats = new();
        private readonly Dictionary<string, RangeStatID> _rangeStats = new();

        private readonly Dictionary<string, WeaponID> _weapons = new();
        private readonly Dictionary<string, ArmorID> _armors = new();

        public bool TryGetRosaryBead(string id, out RosaryBeadItemID bead) => _rosaryBeads.TryGetValue(id, out bead);
        public IEnumerable<RosaryBeadItemID> GetAllRosaryBeads() => _rosaryBeads.Values;

        public bool TryGetPrayer(string id, out PrayerItemID prayer) => _prayers.TryGetValue(id,out prayer);
        public IEnumerable<PrayerItemID> GetAllPrayers() => _prayers.Values;

        public bool TryGetFigurine(string id, out  FigureItemID figurine) => _figurines.TryGetValue(id, out figurine);
        public IEnumerable<FigureItemID> GetAllFigurines() => _figurines.Values;

        public bool TryGetQuestItem(string id, out QuestItemID questItem) => _questItems.TryGetValue(id, out questItem);
        public IEnumerable<QuestItemID> GetAllQuestItems() => _questItems.Values;

        public bool TryGetAbility(string id, out IAbilityTypeRef ability) => _abilities.TryGetValue(id, out ability);
        public IEnumerable<IAbilityTypeRef> GetAllAbilities() => _abilities.Values;

        public bool TryGetValueStat(string id, out ValueStatID valueStat) => _valueStats.TryGetValue(id, out valueStat);

        public bool TryGetRangeStat(string id, out RangeStatID rangeStat) => _rangeStats.TryGetValue(id, out rangeStat);

        public bool TryGetWeapon(string id, out WeaponID weapon) => _weapons.TryGetValue(id, out weapon);
        public IEnumerable<WeaponID> GetAllWeapons() => _weapons.Values;

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
