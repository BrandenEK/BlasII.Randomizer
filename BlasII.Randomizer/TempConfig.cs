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
            this.seed = seed;
            this.startingWeapon = startingWeapon;
        }

        public TempConfig() : this(0, 1) { }
    }
}
