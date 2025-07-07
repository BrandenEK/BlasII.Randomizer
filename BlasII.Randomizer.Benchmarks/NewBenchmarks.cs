using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Settings;
using BlasII.Randomizer.Shuffle;

namespace BlasII.Randomizer.Benchmarks;

public class NewBenchmarks
{
    private Random _rng;
    private IShuffler _shuffler;

    [BenchmarkSetup]
    private void BenchmarkSetup()
    {
        _rng = new Random(102423);
    }

    [BenchmarkSetup(nameof(Shuffle_Reverse))]
    private void BenchmarkSetup_Reverse()
    {
        _shuffler = new PoolsItemShuffler(Core.ItemLocations, Core.Items);
    }

    [BenchmarkSetup(nameof(Shuffle_Forward))]
    private void BenchmarkSetup_Forward()
    {
        _shuffler = new ForwardItemShuffler(Core.ItemLocations, Core.Items);
    }

    [Benchmark("Reverse Fill")]
    [BenchmarkParameters(nameof(SettingsParameters))]
    private BenchmarkResult Shuffle_Reverse(RandomizerSettings settings)
    {
        settings.Seed = SettingsGenerator.GetRandomSeed(_rng);

        var map = new Dictionary<string, string>();
        bool result = _shuffler.Shuffle(settings.Seed, settings, map);

        //Console.WriteLine(message + "Shuffling seed: " + seed);
        return new BenchmarkResult(result, settings, map);
    }

    [Benchmark("Forward Fill")]
    [BenchmarkParameters(nameof(SettingsParameters))]
    private BenchmarkResult Shuffle_Forward(RandomizerSettings settings)
    {
        settings.Seed = SettingsGenerator.GetRandomSeed(_rng);
        
        var map = new Dictionary<string, string>();
        bool result = _shuffler.Shuffle(settings.Seed, settings, map);

        //Console.WriteLine(message + "Shuffling seed: " + seed);
        return new BenchmarkResult(result, settings, map);
    }

    private IEnumerable<SettingsWithDescription> SettingsParameters
    {
        get
        {
            yield return SettingsWithDescription.CreateDefault("Default");
            yield return SettingsWithDescription.CreateDefault("More locations")
                .SetLongQuests(true);
            yield return SettingsWithDescription.CreateDefault("Less locations")
                .SetLongQuests(false);
            yield return SettingsWithDescription.CreateDefault("Start Veredicto")
                .SetStartingWeapon(0);
            yield return SettingsWithDescription.CreateDefault("Start Ruego")
                .SetStartingWeapon(1);
            yield return SettingsWithDescription.CreateDefault("Start Sarmiento")
                .SetStartingWeapon(2);
            yield return SettingsWithDescription.CreateDefault("Start MeaCulpa")
                .SetStartingWeapon(3);
        }
    }
}
