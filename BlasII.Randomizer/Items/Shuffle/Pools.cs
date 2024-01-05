using System;
using System.Collections;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items.Shuffle
{
    internal class BasePool<T> : IEnumerable<T>
    {
        private readonly List<T> _elements;
        private readonly Random _rng;

        /// <summary>
        /// Initializes an empty pool of elements
        /// </summary>
        public BasePool(Random rng)
        {
            _elements = new List<T>();
            _rng = rng;
        }

        /// <summary>
        /// Initializes a pool with all the elements from a different pool
        /// </summary>
        public BasePool(BasePool<T> pool)
        {
            _elements = new List<T>(pool._elements);
            _rng = pool._rng;
        }

        /// <summary>
        /// The number of elements in the pool
        /// </summary>
        public int Size => _elements.Count;

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
        /// Removes a specific element from the pool
        /// </summary>
        public void Remove(T element)
        {
            _elements.Remove(element);
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

        /// <summary>
        /// Removes all elements matching a certain criteria
        /// </summary>
        public void RemoveConditional(Predicate<T> predicate)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (!predicate(_elements[i]))
                    continue;

                _elements.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Moves an element to the beginning of the pool
        /// </summary>
        public void MoveToBeginning(T element)
        {
            _elements.Remove(element);
            _elements.Insert(0, element);
        }

        /// <summary>
        /// Moves an element to the end of the pool
        /// </summary>
        public void MoveToEnd(T element)
        {
            _elements.Remove(element);
            _elements.Add(element);
        }

        /// <summary>
        /// Adds all elements in the other pool to this one
        /// </summary>
        public void Combine(BasePool<T> pool)
        {
            _elements.AddRange(pool);
        }

        public IEnumerator<T> GetEnumerator() => _elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class ItemPool : BasePool<Item>
    {
        public ItemPool(Random rng) : base(rng) { }

        public ItemPool(ItemPool pool) : base(pool) { }
    }

    internal class LocationPool : BasePool<ItemLocation>
    {
        public LocationPool(Random rng) : base(rng) { }

        public LocationPool(LocationPool pool) : base(pool) { }
    }
}
