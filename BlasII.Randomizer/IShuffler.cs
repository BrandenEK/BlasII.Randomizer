using System.Collections.Generic;

namespace BlasII.Randomizer
{
    internal interface IShuffler
    {
        public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output);
    }
}
