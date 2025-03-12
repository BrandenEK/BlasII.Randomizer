
namespace BlasII.Randomizer.Benchmarks.Models;

public class BenchmarkResult
{
    public bool WasSuccessful { get; }

    public Dictionary<string, string> Mapping { get; }

    public BenchmarkResult(bool wasSuccessful, Dictionary<string, string> mapping)
    {
        WasSuccessful = wasSuccessful;
        Mapping = mapping;
    }
}
