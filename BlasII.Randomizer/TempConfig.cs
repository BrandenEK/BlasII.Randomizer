using Newtonsoft.Json;

namespace BlasII.Randomizer
{
    public class TempConfig
    {
        public uint seed;
        public byte startingWeapon;

        public bool shuffleLongQuests;
        public bool shuffleShops;

        [JsonConstructor]
        public TempConfig(uint seed, byte startingWeapon, bool shuffleLongQuests, bool shuffleShops)
        {
            this.seed = ValidateSeed(seed);
            this.startingWeapon = ValidateStartingWeapon(startingWeapon);
            this.shuffleLongQuests = shuffleLongQuests;
            this.shuffleShops = shuffleShops;
        }

        public TempConfig() : this(0, 1, false, true) { }

        private uint ValidateSeed(uint seed)
        {
            return seed > 0 ? seed : (uint)new System.Random().Next(1, RandomizerSettings.MAX_SEED);
        }

        private byte ValidateStartingWeapon(byte weapon)
        {
            return weapon >= 0 && weapon <= 2 ? weapon : (byte) 1;
        }
    }
}
