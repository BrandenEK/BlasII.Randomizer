using System;

namespace BlasII.Randomizer
{
	public class RandomizerSettings
    {
        public const int MAX_SEED = 999_999;

        // General settings
        public readonly int seed;
        public readonly bool allowHints;

        // Item rando settings
        public readonly int logicType;
        public readonly int glitchType;
        public readonly int startingWeapon;
        public readonly int startingLocation;
        public readonly bool shuffleLongQuests;
        public readonly bool shuffleShops;

        // Enemy rando settings
        public readonly int enemyType;

        // Door rando settings
        public readonly int doorType;

        public static RandomizerSettings DefaultSettings => new(RandomSeed, 1, 0, 0, 0, false, true, true, 0, 0);

        public static int RandomSeed => new Random().Next(1, MAX_SEED + 1);

        public RandomizerSettings(int seed, int logic, int glitch, int weapon, int location, bool longQuests, bool shops, bool hints, int enemy, int door)
        {
            this.seed = seed;

            logicType = logic;
            glitchType = glitch;

            startingWeapon = weapon;
            startingLocation = location;
            shuffleLongQuests = longQuests;
            shuffleShops = shops;

            allowHints = hints;

            enemyType = enemy;
            doorType = door;
        }

        public int RealStartingWeapon
        {
            get
            {
                if (startingWeapon >= 1 && startingWeapon <= 3)
                    return (startingWeapon - 1);

                return new Random(seed).Next(0, 3);
            }
        }

        public enum Weapon
        {
            Random,
            Censor,
            Sword,
            Rapier,
        }

        public enum Location
        {
            Repose,
        }
    }
}