using BlasII.Randomizer.Doors;
using LogicParser;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    public class Blas2Inventory : InventoryData
    {
        // Weapons
        int censer, blade, rapier;

        // Abilities
        bool wallClimb, doubleJump, airDash, cherubRings;

        // Bosses
        int bossKeys;

        // Collections
        int tools, tributes, waxSeeds;

        // Hand quest
        int kisses;
        bool brokenKey;

        // Cherub quest
        int cherubs;
        bool rattle;

        // Lullaby quest
        int lullabies;

        // Regula quest
        bool regulaCloth;

        // Yerma quest
        bool holyOil;

        // Elder quest
        bool elderScroll, elderCloth;

        // Letter quest
        bool letter1, letter2, letter3, letter4, letter5;

        int DaughterRooms
        {
            get
            {
                int rooms = 0;
                if (wallClimb && doubleJump) rooms++;
                if (wallClimb && doubleJump && censer > 0 && blade > 0 && rapier > 0) rooms++;
                if (wallClimb && doubleJump && airDash && blade > 0) rooms++;
                if (doubleJump && airDash && cherubRings) rooms++;
                if (wallClimb && doubleJump && airDash && cherubRings && censer > 0 && blade > 0 && rapier > 0) rooms++;

                return rooms;
            }
        }

        protected override object GetVariable(string variable)
        {
            // Door variable
            if (_doors.TryGetValue(variable, out var door))
            {
                return Evaluate(door.logic);
            }

            // Regular variable
            return variable switch
            {
                // Weapons
                "censer" => censer > 0,
                "blade" => blade > 0,
                "rapier" => rapier > 0,

                // Abilities
                "wallClimb" => wallClimb,
                "doubleJump" => doubleJump,
                "airDash" => airDash,
                "cherubRings" => cherubRings,

                // Bosses
                "bossKeys" => bossKeys,

                // Collections
                "tools" => tools,
                "tributes" => tributes,
                "waxSeeds" => waxSeeds,

                // Hand quest
                "kisses" => kisses,
                "brokenKey" => brokenKey,

                // Cherub quest
                "cherubs" => cherubs,
                "rattle" => rattle,

                // Lullaby quest
                "lullabies" => lullabies,

                // Regula quest
                "regulaCloth" => regulaCloth,

                // Yerma quest
                "holyOil" => holyOil,

                // Elder quest
                "elderScroll" => elderScroll,
                "elderCloth" => elderCloth,

                // Letter quest
                "letter1" => letter1,
                "letter2" => letter2,
                "letter3" => letter3,
                "letter4" => letter4,
                "letter5" => letter5,

                // Rooms
                "daughterRooms" => DaughterRooms,

                _ => throw new LogicParserException("Unknown variable: " + variable)
            };
        }

        public void AddItem(Item item)
        {
            switch (item.type)
            {
                case Item.ItemType.QuestItem: ChangeQuestItem(item.id, true); break;
                case Item.ItemType.Weapon: ChangeWeapon(item.id, true); break;
                case Item.ItemType.Ability: ChangeAbility(item.id, true); break;
                case Item.ItemType.Cherub: ChangeCherub(true); break;
            }
        }

        public void RemoveItem(Item item)
        {
            switch (item.type)
            {
                case Item.ItemType.QuestItem: ChangeQuestItem(item.id, false); break;
                case Item.ItemType.Weapon: ChangeWeapon(item.id, false); break;
                case Item.ItemType.Ability: ChangeAbility(item.id, false); break;
                case Item.ItemType.Cherub: ChangeCherub(false); break;
            }
        }

        private void ChangeQuestItem(string id, bool addition)
        {
            int value = addition ? 1 : -1;
            switch (id)
            {
                case "QI05": regulaCloth = addition; break;
                case "QI07": elderScroll = addition; break;
                case "QI08": elderCloth = addition; break;
                case "QI14": letter1 = addition; break;
                case "QI16": letter2 = addition; break;
                case "QI18": letter3 = addition; break;
                case "QI20": letter4 = addition; break;
                case "QI22": letter5 = addition; break;
                case "QI28": brokenKey = addition; break;
                case "QI54": rattle = addition; break;
                case "QI63": bossKeys += value; break;
                case "QI64": bossKeys += value; break;
                case "QI65": bossKeys += value; break;
                case "QI66": bossKeys += value; break;
                case "QI67": bossKeys += value; break;
                case "QI68": holyOil = addition; break;
                case "ST": tools += value; break;
                case "UL": lullabies += value; break;
                case "FT": tributes += value; break;
                case "FK": kisses += value; break;
                case "WS": waxSeeds += value; break;
            }
        }

        private void ChangeWeapon(string id, bool addition)
        {
            int value = addition ? 1 : -1;
            switch (id)
            {
                case "WE01": censer += value; return;
                case "WE02": blade += value; return;
                case "WE03": rapier += value; return;
            }
        }

        private void ChangeAbility(string id, bool addition)
        {
            switch (id)
            {
                case "AB44": wallClimb = addition; return;
                case "AB02": doubleJump = addition; return;
                case "AB01": airDash = addition; return;
                case "AB35": cherubRings = addition; return;
            }
        }

        private void ChangeCherub(bool addition)
        {
            cherubs += addition ? 1 : -1;
        }

        public Blas2Inventory(RandomizerSettings settings, IDictionary<string, DoorLocation> doors)
        {
            _settings = settings;
            _doors = doors;
        }

        private readonly RandomizerSettings _settings;
        private readonly IDictionary<string, DoorLocation> _doors;
    }
}
