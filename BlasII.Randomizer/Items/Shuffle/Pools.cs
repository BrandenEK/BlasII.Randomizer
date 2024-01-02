using System;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items.Shuffle
{
    internal class BasePool<T>
    {
        private readonly List<T> _elements;
        private readonly Random _rng;

        /// <summary>
        /// Initializes an empty pool of elements
        /// </summary>
        public BasePool(Random rng)
        {
            _rng = rng;
        }

        /// <summary>
        /// Adds an element to the end of the pool
        /// </summary>
        public void Add(T element)
        {
            _elements.Add(element);
        }

        /// <summary>
        /// Shuffles the elements in the pool
        /// </summary>
        public void Shuffle()
        {
            int upperIdx = _elements.Count;
            while (upperIdx > 1)
            {
                upperIdx--;
                int randIdx = _rng.Next(upperIdx + 1);
                T value = _elements[randIdx];
                _elements[randIdx] = _elements[upperIdx];
                _elements[upperIdx] = value;
            }
        }

        /// <summary>
        /// Removes the last element from the pool
        /// </summary>
        public T RemoveLast()
        {
            int index = _elements.Count - 1;
            T element = _elements[index];

            _elements.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Removes a random element from the pool
        /// </summary>
        public T RemoveRandom()
        {
            int index = _rng.Next(_elements.Count);
            T element = _elements[index];

            _elements.RemoveAt(index);
            return element;
        }

        //protected T RemoveRandomFromOther<T>(List<T> list, List<T> other)
        //{
        //    T element = RandomElement(list);

        //    other.Remove(element);
        //    return element;
        //}

        //private T RandomElement() => _elements[RandomInteger(_elements.Count)];
    }

    internal class ItemPool : BasePool<Item>
    {
        public ItemPool(Random rng) : base(rng) { }
    }

    internal class LocationPool : BasePool<ItemLocation>
    {
        public LocationPool(Random rng) : base(rng) { }
    }
}
