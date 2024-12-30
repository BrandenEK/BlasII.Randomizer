using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Monitors;
using System.Diagnostics;
using System.Reflection;
using System.Text;

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

        RunAllBenchmarks(benchmark, benchmarkMethods);
        DisplayOutput1(benchmarkMethods);
    }

    static void RunAllBenchmarks(object obj, IEnumerable<MethodInfo> methods)
    {
        Console.WriteLine($"Running {methods.Count()} benchmarks");
        foreach (var method in methods)
        {
            foreach (var setup in GetAllSetups<NewBenchmarks>(method.Name))
                setup.Invoke(obj, null);

            RunBenchmark(obj, method);
        }
    }

    static void RunBenchmark(object obj, MethodInfo method)
    {
        Console.WriteLine($"Running benchmark {method.Name}");

        var watch = new Stopwatch();
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            watch.Restart();
            bool result = (bool)method.Invoke(obj, null);
            watch.Stop();

            foreach (var monitor in _monitors)
                monitor.HandleResult(method.Name, watch.Elapsed, result);
        }
    }

    static void DisplayOutput1(IEnumerable<MethodInfo> methods)
    {
        string[,] output = new string[methods.Count() + 1, _monitors.Count + 1];

        // Add header row
        output[0, 0] = "Method";
        for (int i = 0; i < _monitors.Count; i++)
        {
            output[0, i + 1] = _monitors[i].DisplayName;
        }

        // Add data rows
        int row = 0, col = 0;
        foreach (var method in methods)
        {
            output[++row, 0] = method.Name;
            foreach (var monitor in _monitors)
            {
                output[row, ++col] = monitor.FormatResult(method.Name);
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

    static IEnumerable<MethodInfo> GetAllBenchmarks<TBenchmark>() where TBenchmark : class
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<BenchmarkAttribute>() != null);
    }

    private const int MAX_ITERATIONS = 50;
}
