
namespace BlasII.Randomizer.Benchmarks.Models;

public class BenchmarkResult
{
    public bool Result { get; }

    public Dictionary<string, string> Mapping { get; }

    public BenchmarkResult(bool result, Dictionary<string, string> mapping)
    {
        Result = result;
        Mapping = mapping;
    }
}
