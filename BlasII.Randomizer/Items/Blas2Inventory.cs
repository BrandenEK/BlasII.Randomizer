using LogicParser;

namespace BlasII.Randomizer.Items
{
    internal class Blas2Inventory : InventoryData
    {
        // Weapons
        int censer = 0;
        int blade = 0;
        int rapier = 0;

        // Abilities
        bool wallClimb = false;
        bool doubleJump = false;
        bool airDash = false;
        bool cherubRings = false;

        // Bosses
        int bossKeys = 0;

        // Collections
        int tools = 0;
        int tributes = 0;
        int waxSeeds = 0;

        // Hand quest
        int kisses = 0;
        bool brokenKey = false;

        // Cherub quest
        int cherubs = 0;
        bool rattle = false;

        // Lullaby quest
        int lullabies = 0;

        // Regula quest
        bool regulaCloth = false;

        // Yerma quest
        bool lance = false;
        bool holyOil = false;

        // Elder quest
        bool elderScroll = false;
        bool elderCloth = false;

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
            if (Main.Randomizer.Data.DoesDoorExist(variable))
            {
                return Evaluate(Main.Randomizer.Data.GetDoor(variable).logic);
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
                "lance" => throw new System.Exception("Check for the actual weapon memory instead!"),
                "holyOil" => holyOil,

                // Elder quest
                "elderScroll" => elderScroll,
                "elderCloth" => elderCloth,

                // Rooms
                "daughterRooms" => daughterRooms,

                "???" => false,
                _ => throw new System.Exception("Unknown variable: " + variable)
            };
        }

        public void AddItem(Item item)
        {
            switch (item.type)
            {
                case Item.ItemType.RosaryBead: break;
                case Item.ItemType.Prayer: break;
                case Item.ItemType.Figurine: break;
                case Item.ItemType.QuestItem: AddQuestItem(item.id); break;
                case Item.ItemType.Weapon: AddWeapon(item.id); break;
                case Item.ItemType.Ability: AddAbility(item.id); break;
                case Item.ItemType.Tears: break;
                case Item.ItemType.Marks: break;
            }
        }

        private void AddQuestItem(string id)
        {
            switch (id)
            {
                case "QI05": regulaCloth = true; break;
                case "QI07": elderScroll = true; break;
                case "QI08": elderCloth = true; break;
                case "QI28": brokenKey = true; break;
                case "QI54": rattle = true; break;
                case "QI68": holyOil = true; break;
                case "QI70": lance = true; break;
                case "ST": tools++; break;
                case "UL": lullabies++; break;
                case "FT": tributes++; break;
                case "FK": kisses++; break;
                case "WS": waxSeeds++; break;
                case "BK": bossKeys++; break;
            }
        }

        private void AddWeapon(string id)
        {
            switch (id)
            {
                case "WE01": censer++; return;
                case "WE02": blade++; return;
                case "WE03": rapier++; return;
            }
        }

        private void AddAbility(string id)
        {
            switch (id)
            {
                case "AB44": wallClimb = true; return;
                case "AB02": doubleJump = true; return;
                case "AB01": airDash = true; return;
                case "AB35": cherubRings = true; return;
            }
        }
    }
}
