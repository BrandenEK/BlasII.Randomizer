using System;
using System.Collections.Generic;

namespace BlasII.Randomizer
{
    internal abstract class BaseShuffler
    {
        private Random rng;

        public abstract bool Shuffle(uint seed, TempConfig config, Dictionary<string, string> output);

        protected void Initialize(uint seed) => rng = new Random((int)seed);

        protected int RandomInteger(int max) => rng.Next(max);

        protected void ShuffleList<T>(List<T> list)
        {
            int upperIdx = list.Count;
            while (upperIdx > 1)
            {
                upperIdx--;
                int randIdx = RandomInteger(upperIdx + 1);
                T value = list[randIdx];
                list[randIdx] = list[upperIdx];
                list[upperIdx] = value;
            }
        }
    }
}
