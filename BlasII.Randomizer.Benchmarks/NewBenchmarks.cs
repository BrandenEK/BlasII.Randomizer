using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle;
using Newtonsoft.Json;

namespace BlasII.Randomizer.Benchmarks;

public class NewBenchmarks
{
    private Dictionary<string, ItemLocation> _allItemLocations;
    private Dictionary<string, Item> _allItems;
    private Dictionary<string, Door> _allDoors;

    private Random _rng;
    private IShuffler _shuffler;

    [GlobalSetup]
    private void GlobalSetup()
    {
        string dataFolder = GetDataFolder();

        _allItemLocations = LoadJsonDictionary<ItemLocation>(Path.Combine(dataFolder, "item-locations.json"));
        _allItems = LoadJsonDictionary<Item>(Path.Combine(dataFolder, "items.json"));
        _allDoors = LoadJsonDictionary<Door>(Path.Combine(dataFolder, "doors.json"));
    }

    private string GetDataFolder()
    {
        string currentDir = Environment.CurrentDirectory;
        string baseDir = currentDir.Substring(0, currentDir.IndexOf("BlasII.Randomizer.Benchmarks"));
        return Path.Combine(baseDir, "resources", "data", "Randomizer");
    }

    private Dictionary<string, T> LoadJsonDictionary<T>(string path) where T : IUnique
    {
        string json = File.ReadAllText(path);
        T[] values = JsonConvert.DeserializeObject<T[]>(json);

        return values.ToDictionary(x => x.Id, x => x);
    }

    [BenchmarkSetup]
    private void BenchmarkSetup()
    {
        _rng = new Random(102423);
        _shuffler = new PoolsItemShuffler(_allItemLocations, _allItems);
    }

    [Benchmark]
    private bool Pools_Default()
    {
        int seed = _rng.Next(1, RandomizerSettings.MAX_SEED + 1);
        var map = new Dictionary<string, string>();
        var settings = RandomizerSettings.DEFAULT;

        //Console.WriteLine("Shuffling seed: " + seed);
        return _shuffler.Shuffle(seed, settings, map);
    }

    [Benchmark]
    private bool Pools_MoreLocs()
    {
        int seed = _rng.Next(1, RandomizerSettings.MAX_SEED + 1);
        var map = new Dictionary<string, string>();
        var settings = new RandomizerSettings()
        {
            ShuffleLongQuests = true,
            ShuffleShops = true,
            StartingWeapon = 0,
        };

        //Console.WriteLine("Shuffling seed: " + seed);
        return _shuffler.Shuffle(seed, settings, map);
    }
}
