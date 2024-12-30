using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Benchmarks.Monitors;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    private readonly static List<BaseMonitor> _monitors = new();

    static void Main(string[] args)
    {
        object obj = new NewBenchmarks();
        var benchmarks = FindAllBenchmarks<NewBenchmarks>();

        RegisterMonitors(new SuccessRateMonitor(), new AverageTimeMonitor(), new AverageSuccessTimeMonitor());
        RunAllBenchmarks(obj, benchmarks);
        DisplayOutput1(benchmarks);
    }

    public static void RegisterMonitors(params BaseMonitor[] monitors)
    {
        _monitors.AddRange(monitors);
    }

    static List<BenchmarkInfo> FindAllBenchmarks<T>()
    {
        var benchmarks = new List<BenchmarkInfo>();
        var methods = typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            BenchmarkAttribute attribute = method.GetCustomAttribute<BenchmarkAttribute>();

            if (attribute == null)
                continue;

            benchmarks.Add(new BenchmarkInfo(method.Name, attribute.Name ?? method.Name, method));
        }

        return benchmarks;
    }

    static void RunAllBenchmarks(object obj, List<BenchmarkInfo> benchmarks)
    {
        Console.WriteLine($"Running {benchmarks.Count} benchmarks");
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in GetAllSetups<NewBenchmarks>(benchmark.Id))
                setup.Invoke(obj, null);

            RunBenchmark(obj, benchmark);
        }
    }

    static void RunBenchmark(object obj, BenchmarkInfo benchmark)
    {
        Console.WriteLine($"Running benchmark {benchmark.Id}");

        var watch = new Stopwatch();
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            watch.Restart();
            bool result = (bool)benchmark.Method.Invoke(obj, null);
            watch.Stop();

            foreach (var monitor in _monitors)
                monitor.HandleResult(benchmark.Id, watch.Elapsed, result);
        }
    }

    static void DisplayOutput1(List<BenchmarkInfo> benchmarks)
    {
        string[,] output = new string[benchmarks.Count + 1, _monitors.Count + 1];

        // Add header row
        output[0, 0] = "Method";
        for (int i = 0; i < _monitors.Count; i++)
        {
            output[0, i + 1] = _monitors[i].DisplayName;
        }

        // Add data rows
        int row = 0, col = 0;
        foreach (var benchmark in benchmarks)
        {
            output[++row, 0] = benchmark.Name;
            foreach (var monitor in _monitors)
            {
                output[row, ++col] = monitor.FormatResult(benchmark.Id);
            }
            col = 0;
        }

        DisplayOutput2(output);
    }

    static void DisplayOutput2(string[,] output)
    {
        Console.WriteLine();
        var sbs = new StringBuilder[output.GetLength(0)];
        for (int i = 0; i < sbs.Length; i++)
            sbs[i] = new StringBuilder("|");

        for (int col = 0; col < output.GetLength(1); col++)
        {
            // Find maxwidth in the column
            int maxWidth = 0;
            for (int row = 0; row < output.GetLength(0); row++)
            {
                int width = output[row, col].Length;
                if (width > maxWidth)
                    maxWidth = width;
            }

            // Add each row's text
            for (int row = 0; row < output.GetLength(0); row++)
            {
                sbs[row].Append(output[row, col].PadLeft(maxWidth + 1, ' ')).Append(" |");
            }
        }

        string header = sbs[0].ToString();
        var line = new StringBuilder();
        Console.WriteLine(header);

        foreach (char c in header)
        {
            line.Append(c == '|' ? '|' : '-');
        }
        Console.WriteLine(line);

        foreach (var sb in sbs.Skip(1))
        {
            Console.WriteLine(sb);
        }
    }

    static IEnumerable<MethodInfo> GetAllSetups<TBenchmark>(string target) where TBenchmark : class
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => HasValidSetupAttribute(x, target));
    }

    static bool HasValidSetupAttribute(MethodInfo method, string target)
    {
        BenchmarkSetupAttribute attribute = method.GetCustomAttribute<BenchmarkSetupAttribute>();
        return attribute != null && (string.IsNullOrEmpty(attribute.Target) || attribute.Target == target);
    }

    private const int MAX_ITERATIONS = 100;
}
