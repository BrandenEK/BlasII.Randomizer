using BlasII.Randomizer.Doors;
using LogicParser;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    public class Blas2InventoryReverse : InventoryData
    {
        // Weapons
        int censer = 3;
        int blade = 3;
        int rapier = 3;

        // Abilities
        bool wallClimb = true;
        bool doubleJump = true;
        bool airDash = true;
        bool cherubRings = true;

        // Bosses
        int bossKeys = 5;

        // Collections
        int tools = 5;
        int tributes = 3;
        int waxSeeds = 6;

        // Hand quest
        int kisses = 5;
        bool brokenKey = true;

        // Cherub quest
        int cherubs = 33;
        bool rattle = true;

        // Lullaby quest
        int lullabies = 5;

        // Regula quest
        bool regulaCloth = true;

        // Yerma quest
        bool holyOil = true;

        // Elder quest
        bool elderScroll = true;
        bool elderCloth = true;

        // Letter quest
        bool letter1 = true, letter2 = true, letter3 = true, letter4 = true, letter5 = true;

        int daughterRooms
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
                "daughterRooms" => daughterRooms,

                _ => throw new LogicParserException("Unknown variable: " + variable)
            };
        }

        public void RemoveItem(Item item)
        {
            switch (item.type)
            {
                case Item.ItemType.QuestItem: RemoveQuestItem(item.id); break;
                case Item.ItemType.Weapon: RemoveWeapon(item.id); break;
                case Item.ItemType.Ability: RemoveAbility(item.id); break;
                case Item.ItemType.Cherub: RemoveCherub(); break;
            }
        }

        private void RemoveQuestItem(string id)
        {
            switch (id)
            {
                case "QI05": regulaCloth = false; break;
                case "QI07": elderScroll = false; break;
                case "QI08": elderCloth = false; break;
                case "QI14": letter1 = false; break;
                case "QI16": letter2 = false; break;
                case "QI18": letter3 = false; break;
                case "QI20": letter4 = false; break;
                case "QI22": letter5 = false; break;
                case "QI28": brokenKey = false; break;
                case "QI54": rattle = false; break;
                case "QI63": bossKeys--; break;
                case "QI64": bossKeys--; break;
                case "QI65": bossKeys--; break;
                case "QI66": bossKeys--; break;
                case "QI67": bossKeys--; break;
                case "QI68": holyOil = false; break;
                case "ST": tools--; break;
                case "UL": lullabies--; break;
                case "FT": tributes--; break;
                case "FK": kisses--; break;
                case "WS": waxSeeds--; break;
            }
        }

        private void RemoveWeapon(string id)
        {
            switch (id)
            {
                case "WE01": censer--; return;
                case "WE02": blade--; return;
                case "WE03": rapier--; return;
            }
        }

        private void RemoveAbility(string id)
        {
            switch (id)
            {
                case "AB44": wallClimb = false; return;
                case "AB02": doubleJump = false; return;
                case "AB01": airDash = false; return;
                case "AB35": cherubRings = false; return;
            }
        }

        private void RemoveCherub()
        {
            cherubs--;
        }

        public Blas2InventoryReverse(RandomizerSettings settings, IDictionary<string, DoorLocation> doors)
        {
            _settings = settings;
            _doors = doors;
        }

        private readonly RandomizerSettings _settings;
        private readonly IDictionary<string, DoorLocation> _doors;
    }
}
