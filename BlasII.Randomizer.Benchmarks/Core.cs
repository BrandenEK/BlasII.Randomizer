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
        var benchmarkMethods = GetAllBenchmarks<NewBenchmarks>();

        _monitors = new List<BaseMonitor>()
        {
            new SuccessRateMonitor(),
            new AverageTimeMonitor(),
            new AverageSuccessTimeMonitor(),
        };

        MethodInfo globalSetup = GetSetupMethod<NewBenchmarks, GlobalSetupAttribute>();
        globalSetup?.Invoke(benchmark, null);

        RunGlobal(benchmark, benchmarkMethods);

        foreach (var monitor in _monitors)
        {
            Console.WriteLine($"{monitor.DisplayName}: {monitor.FormatResult(nameof(ShufflerBenchmarks.Shuffle_Pools))}");
        }
    }

    static void RunGlobal(NewBenchmarks benchmark, IEnumerable<MethodInfo> methods)
    {
        MethodInfo setup = GetSetupMethod<NewBenchmarks, BenchmarkSetupAttribute>();

        Console.WriteLine($"Running {methods.Count()} benchmarks");
        foreach (var method in methods)
        {
            setup?.Invoke(benchmark, null);

            RunBenchmark(benchmark, method);
        }
    }

    static void RunBenchmark(NewBenchmarks benchmark, MethodInfo method)
    {
        MethodInfo setup = GetSetupMethod<NewBenchmarks, IterationSetupAttribute>();

        Console.WriteLine($"Running benchmark {method.Name}");
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            setup?.Invoke(benchmark, null);

            RunIteration(benchmark, method);
        }
    }

    static void RunIteration(NewBenchmarks benchmark, MethodInfo method)
    {
        var watch = Stopwatch.StartNew();
        bool result = (bool)method.Invoke(benchmark, null);
        watch.Stop();

        foreach (var monitor in _monitors)
            monitor.HandleResult(method.Name, watch.Elapsed, result);
    }

    static MethodInfo GetSetupMethod<TBenchmark, TAttribute>() where TBenchmark : class where TAttribute : Attribute
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(x => x.GetCustomAttribute<TAttribute>(false) != null);
    }

    static IEnumerable<MethodInfo> GetAllBenchmarks<TBenchmark>() where TBenchmark : class
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<BenchmarkAttribute>() != null);
    }

    private const int MAX_ITERATIONS = 50;
}
