using Newtonsoft.Json;
using System;
using System.Text;

namespace BlasII.Randomizer
{
	public class RandomizerSettings
    {
        public const int MAX_SEED = 999_999;

        // General settings
        [JsonProperty] public readonly int seed;
        [JsonProperty] public readonly bool allowHints;

        // Item rando settings
        [JsonProperty] public readonly int logicType;
        [JsonProperty] public readonly int requiredKeys;
        [JsonProperty] public readonly int startingWeapon;
        [JsonProperty] public readonly bool shuffleLongQuests;
        [JsonProperty] public readonly bool shuffleShops;

        // Not implemented
        [JsonProperty] public readonly int glitchType;
        [JsonProperty] public readonly int startingLocation;

        // Enemy rando settings
        [JsonProperty] public readonly int enemyType;

        // Door rando settings
        [JsonProperty] public readonly int doorType;

        public static RandomizerSettings DefaultSettings => new(RandomSeed, 1, 5, 0, 0, 0, false, true, true, 0, 0);

        public static int RandomSeed => new Random().Next(1, MAX_SEED + 1);

        [JsonConstructor]
        public RandomizerSettings(int seed, int logic, int keys, int glitch, int weapon, int location, bool longQuests, bool shops, bool hints, int enemy, int door)
        {
            this.seed = seed;
            allowHints = hints;

            logicType = logic;
            requiredKeys = keys;
            startingWeapon = weapon;
            shuffleLongQuests = longQuests;
            shuffleShops = shops;

            glitchType = glitch;
            startingLocation = location;

            enemyType = enemy;
            doorType = door;
        }

        public string FormatInfo()
        {
            string logic = logicType == 0 ? "Easy" : logicType == 1 ? "Normal" : "Hard";
            string keys = requiredKeys > 0 ? (requiredKeys - 1).ToString() : "Random";
            string[] weapons = new string[] { "Veredicto", "Ruego", "Sarmiento" };
            string weapon = startingWeapon > 0 ? weapons[startingWeapon - 1] : "Random";

            var sb = new StringBuilder();
            sb.AppendLine("RANDOMIZER SETTINGS");
            sb.AppendLine("Seed: " + seed);
            sb.AppendLine();
            sb.AppendLine("Logic difficulty: " + logic);
            sb.AppendLine("Required keys: " + keys);
            sb.AppendLine("Starting weapon: " + weapon);
            sb.AppendLine("Shuffle long quests: " + shuffleLongQuests);
            sb.AppendLine("Shuffle shops: " + shuffleShops);

            return sb.ToString();
        }

        public string FormatSpoiler()
        {
            string logic = logicType == 0 ? "Easy" : logicType == 1 ? "Normal" : "Hard";
            string keys = requiredKeys > 0 ? (requiredKeys - 1).ToString() : $"[{RealRequiredKeys}]";
            string[] weapons = new string[] { "Veredicto", "Ruego", "Sarmiento" };
            string weapon = startingWeapon > 0 ? weapons[startingWeapon - 1] : $"[{weapons[RealStartingWeapon]}]";
            var line = new string('=', 35);

            var sb = new StringBuilder();
            sb.AppendLine("Seed: " + seed);
            sb.AppendLine();
            sb.AppendLine(line);
            sb.AppendLine(" Logic difficulty: " + logic);
            sb.AppendLine(" Required keys: " + keys);
            sb.AppendLine(" Starting weapon: " + weapon);
            sb.AppendLine(" Shuffle long quests: " + shuffleLongQuests);
            sb.AppendLine(" Shuffle shops: " + shuffleShops);
            sb.AppendLine(line);

            return sb.ToString();
        }

        [JsonIgnore]
        public int RealStartingWeapon
        {
            get
            {
                if (startingWeapon >= 1 && startingWeapon <= 3)
                    return startingWeapon - 1;

                return new Random(seed).Next(0, 3);
            }
        }

        [JsonIgnore]
        public int RealRequiredKeys
        {
            get
            {
                if (requiredKeys >= 1 && requiredKeys <= 6)
                    return requiredKeys - 1;

                return new Random(seed).Next(0, 6);
            }
        }
    }
}