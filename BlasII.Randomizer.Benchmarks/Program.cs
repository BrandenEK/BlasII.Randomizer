using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<ShufflerBenchmarks>();
    }
}
