using System;
using System.Collections.Generic;

namespace BlasII.Randomizer
{
    internal abstract class BaseShuffler
    {
        private Random rng;

        public abstract bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output);

        protected void Initialize(int seed) => rng = new Random(seed);

        protected int RandomInteger(int max) => rng.Next(max);

        protected T RandomElement<T>(List<T> list) => list[RandomInteger(list.Count)];

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

        protected T RemoveRandom<T>(List<T> list)
        {
            int index = RandomInteger(list.Count);
            T element = list[index];

            list.RemoveAt(index);
            return element;
        }

        protected T RemoveRandomFromOther<T>(List<T> list, List<T> other)
        {           
            T element = RandomElement(list);

            other.Remove(element);
            return element;
        }

        protected T RemoveLast<T>(List<T> list)
        {
            int index = list.Count - 1;
            T element = list[index];

            list.RemoveAt(index);
            return element;
        }
    }
}
