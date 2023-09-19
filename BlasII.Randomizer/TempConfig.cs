using Newtonsoft.Json;

namespace BlasII.Randomizer
{
    public class TempConfig
    {
        public uint seed;
        public byte startingWeapon;

        [JsonConstructor]
        public TempConfig(uint seed, byte startingWeapon)
        {
            this.seed = ValidateSeed(seed);
            this.startingWeapon = ValidateStartingWeapon(startingWeapon);
        }

        public TempConfig() : this(0, 1) { }

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
