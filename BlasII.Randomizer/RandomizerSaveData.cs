using BlasII.ModdingAPI.Persistence;
using System.Collections.Generic;

namespace BlasII.Randomizer
{
    internal class RandomizerSaveData : SaveData
    {
        public Dictionary<string, string> mappedItems;

        public TempConfig tempConfig;
        public string[] collectedLocations;
    }
}
