using BlasII.Randomizer.Benchmarks.Attributes;

namespace BlasII.Randomizer.Benchmarks;

public class NewBenchmarks
{
    [GlobalSetup]
    private void GlobalSetup()
    {
        // Ran once at the beginning
        Console.WriteLine("GlobalSetup");
    }

    [BenchmarkSetup]
    private void BenchmarkSetup()
    {
        // Ran once for every benchmark
        Console.WriteLine("BenchmarkSetup");
    }

    [IterationSetup]
    private void IterationSetup()
    {
        // Ran once for every iteration of every benchmark
        Console.WriteLine("IterationSetup");
    }
}
