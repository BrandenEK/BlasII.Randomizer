using BlasII.Randomizer.Benchmarks.Attributes;

namespace BlasII.Randomizer.Benchmarks;

public class NewBenchmarks
{
    [GlobalSetup]
    private void GlobalSetup()
    {
        // Ran once at the beginning
    }

    [BenchmarkSetup]
    private void BenchmarkSetup()
    {
        // Ran once for every benchmark
    }

    [IterationSetup]
    private void IterationSetup()
    {
        // Ran once for every iteration of every benchmark
    }

    [Benchmark]
    private bool Benchmark1()
    {
        return true;
    }

    [Benchmark]
    private bool Benchmark2()
    {
        return true;
    }
}
