
namespace BlasII.Randomizer.Benchmarks.Models;

public class BenchmarkResult
{
    public bool WasSuccessful { get; }

    public RandomizerSettings Settings { get; }

    public Dictionary<string, string> Mapping { get; }

    public BenchmarkResult(bool wasSuccessful, RandomizerSettings settings, Dictionary<string, string> mapping)
    {
        WasSuccessful = wasSuccessful;
        Settings = settings;
        Mapping = mapping;
    }
}
