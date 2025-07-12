using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Settings;
using BlasII.Randomizer.Shuffle;
using BlasII.Randomizer.Shuffle.Implementations;

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
        _shuffler = new ComponentShuffler(Core.ItemLocations, Core.Items, true);
    }

    [BenchmarkSetup(nameof(Shuffle_Forward))]
    private void BenchmarkSetup_Forward()
    {
        _shuffler = new ComponentShuffler(Core.ItemLocations, Core.Items, false);
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
            yield return new SettingsWithDescription("Default");
            {

            }
            yield return new SettingsWithDescription("Start Veredicto")
            {
                StartingWeapon = 0
            };
            yield return new SettingsWithDescription("Start Ruego")
            {
                StartingWeapon = 1
            };
            yield return new SettingsWithDescription("Start Sarmiento")
            {
                StartingWeapon = 2
            };
            yield return new SettingsWithDescription("Start MeaCulpa")
            {
                StartingWeapon = 3
            };
            yield return new SettingsWithDescription("Vanilla cherubs")
            {
                ShuffleCherubs = false
            };
        }
    }
}
