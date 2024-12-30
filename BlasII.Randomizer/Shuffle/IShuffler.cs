using System.Collections.Generic;

namespace BlasII.Randomizer.Shuffle;

public interface IShuffler
{
    public bool Shuffle(int seed, RandomizerSettings settings, Dictionary<string, string> output);
}
