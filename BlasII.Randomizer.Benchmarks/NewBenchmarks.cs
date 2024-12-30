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
    private RandomizerSettings _settings;

    public NewBenchmarks()
    {
        string dataFolder = GetDataFolder();

        _allItemLocations = LoadJsonDictionary<ItemLocation>(Path.Combine(dataFolder, "item-locations.json"));
        _allItems = LoadJsonDictionary<Item>(Path.Combine(dataFolder, "items.json"));
        _allDoors = LoadJsonDictionary<Door>(Path.Combine(dataFolder, "doors.json"));
        Console.WriteLine($"Item locations: {_allItemLocations.Count}, Items: {_allItems.Count}, Doors: {_allDoors.Count}");
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

    [BenchmarkSetup(nameof(Pools_Default))]
    private void SetupDefault()
    {
        _settings = RandomizerSettings.DEFAULT;
    }

    [BenchmarkSetup(nameof(Pools_MoreLocs))]
    private void SetupMoreLocs()
    {
        _settings = new RandomizerSettings()
        {
            ShuffleLongQuests = true,
            ShuffleShops = true,
            StartingWeapon = 0,
        };
    }

    private bool PerformShuffle()
    {
        int seed = _rng.Next(1, RandomizerSettings.MAX_SEED + 1);
        var map = new Dictionary<string, string>();

        Console.WriteLine("Shuffling seed: " + seed);
        return _shuffler.Shuffle(seed, _settings, map);
    }

    [Benchmark]
    private bool Pools_Default() => PerformShuffle();

    [Benchmark]
    private bool Pools_MoreLocs() => PerformShuffle();
}
