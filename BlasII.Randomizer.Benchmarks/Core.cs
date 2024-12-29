using BlasII.Randomizer.Benchmarks.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    private static List<BaseMonitor> _monitors;

    static void Main(string[] args)
    {
        var benchmark = new NewBenchmarks();

        _monitors = new List<BaseMonitor>()
        {
            new SuccessRateMonitor(),
            new AverageTimeMonitor(),
            new AverageSuccessTimeMonitor(),
        };

        MethodInfo globalSetup = GetSetupMethod<NewBenchmarks, GlobalSetupAttribute>();
        globalSetup?.Invoke(benchmark, null);

        RunGlobal(benchmark);

        foreach (var monitor in _monitors)
        {
            Console.WriteLine($"{monitor.DisplayName}: {monitor.FormatResult(nameof(ShufflerBenchmarks.Shuffle_Pools))}");
        }
    }

    static void RunGlobal(NewBenchmarks benchmark)
    {
        MethodInfo setup = GetSetupMethod<NewBenchmarks, BenchmarkSetupAttribute>();

        // Foreach benchmark
        setup?.Invoke(benchmark, null);
        RunBenchmark(benchmark);
    }

    static void RunBenchmark(NewBenchmarks benchmark)
    {
        MethodInfo setup = GetSetupMethod<NewBenchmarks, IterationSetupAttribute>();

        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            setup?.Invoke(benchmark, null);

            RunIteration(benchmark);
        }
    }

    static void RunIteration(NewBenchmarks benchmark)
    {
        var watch = Stopwatch.StartNew();
        bool result = true;// benchmark.Shuffle_Pools();
        watch.Stop();

        foreach (var monitor in _monitors)
            monitor.HandleResult(nameof(ShufflerBenchmarks.Shuffle_Pools), watch.Elapsed, result);
    }

    static MethodInfo GetSetupMethod<TBenchmark, TAttribute>() where TBenchmark : class where TAttribute : Attribute
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(x => x.GetCustomAttribute<TAttribute>(false) != null);
    }

    private const int MAX_ITERATIONS = 50;
}
