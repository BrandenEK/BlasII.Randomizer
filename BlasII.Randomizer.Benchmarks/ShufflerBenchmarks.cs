using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle;
using Newtonsoft.Json;

namespace BlasII.Randomizer.Benchmarks;

public class ShufflerBenchmarks
{
    private Random _rng;
    private IShuffler _shuffler;
    private RandomizerSettings _settings;

    [GlobalSetup]
    public void SetupShuffler()
    {
        string dataFolder = GetDataFolder();

        var allItemLocations = LoadJsonDictionary<ItemLocation>(Path.Combine(dataFolder, "item-locations.json"));
        var allItems = LoadJsonDictionary<Item>(Path.Combine(dataFolder, "items.json"));
        //var allDoors = LoadJsonDictionary<Door>(Path.Combine(dataFolder, "doors.json"));

        _rng = new Random(102423);
        _shuffler = new PoolsItemShuffler(allItemLocations, allItems);
        _settings = RandomizerSettings.DEFAULT;
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

    //[Benchmark(Description = "PoolsItemShuffler")]
    public bool Shuffle_Pools()
    {
        var map = new Dictionary<string, string>();
        int seed = _rng.Next(1, RandomizerSettings.MAX_SEED + 1);

        Console.WriteLine("Shuffling seed: " + seed);
        return _shuffler.Shuffle(seed, _settings, map);
    }
}
