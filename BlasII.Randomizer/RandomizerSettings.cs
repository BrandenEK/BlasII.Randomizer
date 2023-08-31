using System;

namespace BlasII.Randomizer
{
	public class RandomizerSettings
    {
        public const int MAX_SEED = 999_999;

        // General settings
        public readonly int seed;
        public readonly bool unlockFastTravel;
        public readonly bool allowHints;

        // Item rando settings
        public readonly byte logicType;
        public readonly byte glitchType;
        public readonly Weapon startingWeapon;
        public readonly Location startingLocation;
        public readonly bool junkLongQuests;

        // Enemy rando settings
        public readonly byte enemyType;

        // Door rando settings
        public readonly byte doorType;

        public static RandomizerSettings DefaultSettings => new RandomizerSettings(new Random().Next(1, MAX_SEED), 1, 0, Weapon.Random, Location.Repose, true, true, true, 0, 0);

        public RandomizerSettings(int seed, byte logic, byte glitch, Weapon weapon, Location location, bool junk, bool fast, bool hints, byte enemy, byte door)
        {
            this.seed = seed;

            logicType = logic;
            glitchType = glitch;

            startingWeapon = weapon;
            startingLocation = location;
            junkLongQuests = junk;

            unlockFastTravel = fast;
            allowHints = hints;

            enemyType = enemy;
            doorType = door;
        }

        public enum Weapon
        {
            Censor,
            Sword,
            Rapier,
            Random,
        }

        public enum Location
        {
            Repose,
        }
    }
}