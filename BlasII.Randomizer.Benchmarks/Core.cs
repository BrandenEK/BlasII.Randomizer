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
        var cmd = new BenchmarkCommand();
        cmd.Process(args);

        object obj = new NewBenchmarks();
        var benchmarks = FindAllBenchmarks<NewBenchmarks>(obj);

        RegisterMonitors(new SuccessRateMonitor(), new AverageTimeMonitor(), new AverageSuccessTimeMonitor());
        RunAllWarmups(obj, benchmarks);
        RunAllBenchmarks(obj, benchmarks);
        DisplayOutput1(benchmarks);

        if (cmd.WaitForInput)
            Console.ReadKey(true);
    }

    public static void RegisterMonitors(params BaseMonitor[] monitors)
    {
        _monitors.AddRange(monitors);
    }

    static List<BenchmarkInfo> FindAllBenchmarks<T>(object obj)
    {
        var benchmarks = new List<BenchmarkInfo>();
        var methods = typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            BenchmarkAttribute bAttribute = method.GetCustomAttribute<BenchmarkAttribute>();

            if (bAttribute == null)
                continue;

            BenchmarkParametersAttribute bpAttribute = method.GetCustomAttribute<BenchmarkParametersAttribute>();

            if (bpAttribute == null)
            {
                benchmarks.Add(new BenchmarkInfo(method.Name, bAttribute.Name ?? method.Name, method, null));
                continue;
            }

            IEnumerable<object> parameters = bpAttribute.ParameterProperty != null
                ? GetParameters<T>(obj, bpAttribute.ParameterProperty)
                : bpAttribute.Parameters ?? throw new Exception("You have to add the parameter property or the list");

            benchmarks.AddRange(parameters
                .Select(x => new BenchmarkInfo($"{method.Name}.{x}", bAttribute.Name ?? method.Name, method, new object[] { x })));
        }

        return benchmarks;
    }

    static void RunAllWarmups(object obj, List<BenchmarkInfo> benchmarks)
    {
        Console.WriteLine($"Running {benchmarks.Count} warmups");
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in GetAllSetups<NewBenchmarks>(benchmark.Id))
                setup.Invoke(obj, null);

            RunWarmup(obj, benchmark);
        }
    }

    static void RunWarmup(object obj, BenchmarkInfo benchmark)
    {
        Console.WriteLine($"Running warmup {benchmark.Id}");

        for (int i = 0; i < MAX_ITERATIONS_WARMUP; i++)
        {
            benchmark.Method.Invoke(obj, benchmark.Parameters);
        }
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
            bool result = (bool)benchmark.Method.Invoke(obj, benchmark.Parameters);
            watch.Stop();

            foreach (var monitor in _monitors)
                monitor.HandleResult(benchmark.Id, watch.Elapsed, result);
        }
    }

    static void DisplayOutput1(List<BenchmarkInfo> benchmarks)
    {
        string[,] output = new string[benchmarks.Count + 1, _monitors.Count + 2];

        // Add header row
        output[0, 0] = "Method";
        output[0, 1] = "Parameters";
        for (int i = 0; i < _monitors.Count; i++)
        {
            output[0, i + 2] = _monitors[i].DisplayName;
        }

        // Add data rows
        int row = 0, col = 0;
        foreach (var benchmark in benchmarks)
        {
            output[++row, col] = benchmark.Name;
            output[row, ++col] = benchmark.Parameters?[0].ToString() ?? string.Empty;
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

    static IEnumerable<object> GetParameters<T>(object obj, string name)
    {
        PropertyInfo property = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        return property == null
            ? throw new Exception($"Failed to find property with the name {name}")
            : (IEnumerable<object>)property.GetValue(obj);
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
    private const int MAX_ITERATIONS_WARMUP = 40;
}
