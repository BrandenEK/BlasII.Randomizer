using System.Diagnostics;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    static void Main(string[] args)
    {
        int attemptsPerBenchmark = 100;

        var benchmark = new ShufflerBenchmarks();
        benchmark.SetupShuffler();

        var monitors = new List<BaseMonitor>()
        {
            new SuccessRateMonitor(),
            new AverageTimeMonitor(),
            new AverageSuccessTimeMonitor(),
        };

        var watch = new Stopwatch();
        for (int i = 0; i < attemptsPerBenchmark; i++)
        {
            watch.Restart();
            bool result = benchmark.Shuffle_Pools();
            watch.Stop();

            foreach (var monitor in monitors)
                monitor.HandleResult(nameof(ShufflerBenchmarks.Shuffle_Pools), watch.Elapsed, result);
        }

        foreach (var monitor in monitors)
        {
            Console.WriteLine($"{monitor.DisplayName}: {monitor.FormatResult(nameof(ShufflerBenchmarks.Shuffle_Pools))}");
        }
    }
}
