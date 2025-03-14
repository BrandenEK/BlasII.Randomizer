using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Exporters;
using BlasII.Randomizer.Benchmarks.Metrics;
using BlasII.Randomizer.Benchmarks.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    private static IMetric<BenchmarkResult>[] _metrics = Array.Empty<IMetric<BenchmarkResult>>();
    private static readonly List<IExporter> _exporters = new();

    static void Main(string[] args)
    {
        var cmd = new BenchmarkCommand();
        cmd.Process(args);

        RegisterMetrics(new SuccessRateMetric(), new AverageTimeMetric(), new AverageSuccessTimeMetric());
        AddExporter(new ConsoleExporter());
        if (cmd.ExportResults)
            AddExporter(new FileExporter(Path.Combine(BASE_DIRECTORY, "BenchmarkResults")));

        object obj = new NewBenchmarks();
        var benchmarks = FindAllBenchmarks<NewBenchmarks>(obj);

        if (!cmd.SkipWarmup)
            RunAllWarmups(obj, benchmarks, cmd.MaxIterations / 5);

        var results = new string[benchmarks.Count + 1, _metrics.Length + 2];
        RunAllBenchmarks(obj, benchmarks, cmd.MaxIterations, results);

        string display = GetInfoDisplay(cmd.MaxIterations) + GetResultsDisplay(results);

        foreach (var exporter in _exporters)
            exporter.Export(display);

        if (cmd.WaitForInput)
            Console.ReadKey(true);
    }

    private static string GetInfoDisplay(int iterationCount)
    {
        var text = new List<string>()
        {
            $" Machine: {Environment.MachineName} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")} ({Environment.ProcessorCount} processors)",
            $" Operating system: {Environment.OSVersion}",
            $" Start time: {DateTime.Now:MM/dd/yy H:mm:ss}",
            $" Max iterations: {iterationCount}",
            $" Debug mode: {Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled)}",
        };

        var line = new string('=', text.Max(x => x.Length) + 1);

        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine(line);
        sb.AppendJoin(Environment.NewLine, text);
        sb.AppendLine();
        sb.AppendLine(line);
        sb.AppendLine();

        return sb.ToString();
    }

    private static string GetResultsDisplay(string[,] results)
    {
        var sb = new StringBuilder();

        // Fill header row
        results[0, 0] = "Method";
        results[0, 1] = "Parameters";
        for (int i = 0; i < _metrics.Length; i++)
            results[0, i + 2] = _metrics[i].DisplayName;

        // Calculate maxwidth for each column
        var columnWidths = new int[results.GetLength(1)];
        for (int y = 0; y < results.GetLength(1); y++)
        {
            for (int x = 0; x < results.GetLength(0); x++)
            {
                if (results[x, y].Length > columnWidths[y])
                    columnWidths[y] = results[x, y].Length;
            }
        }

        // Add formatted data to the outout
        for (int x = 0; x < results.GetLength(0); x++)
        {
            char padding = x == 0 ? '-' : ' ';

            sb.Append('|');
            for (int y = 0; y < results.GetLength(1); y++)
            {
                sb.Append($" {results[x, y].PadLeft(columnWidths[y], ' ')} |");
            }
            sb.AppendLine();

            if (x >= results.GetLength(0) - 1 || results[x, 0] == results[x + 1, 0])
                continue;

            sb.Append('|');
            for (int y = 0; y < results.GetLength(1); y++)
            {
                sb.Append($"{new string(padding, columnWidths[y] + 2)}|");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static void RegisterMetrics(params IMetric<BenchmarkResult>[] metrics)
    {
        _metrics = metrics;
    }

    public static void AddExporter(IExporter exporter)
    {
        _exporters.Add(exporter);
    }

    public static void AddExporters(params IExporter[] exporters)
    {
        _exporters.AddRange(exporters);
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
                ? ReflectionHelper.GetParameters<T>(obj, bpAttribute.ParameterProperty)
                : bpAttribute.Parameters ?? throw new Exception("You have to add the parameter property or the list");

            benchmarks.AddRange(parameters
                .Select(x => new BenchmarkInfo($"{method.Name}.{x}", bAttribute.Name ?? method.Name, method, new object[] { x })));
        }

        return benchmarks;
    }

    static void RunAllWarmups(object obj, List<BenchmarkInfo> benchmarks, int iterationCount)
    {
        Console.WriteLine($"Running {benchmarks.Count} warmups");
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in ReflectionHelper.GetAllSetups<NewBenchmarks>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            RunWarmup(obj, benchmark, iterationCount);
        }
    }

    static void RunWarmup(object obj, BenchmarkInfo benchmark, int iterationCount)
    {
        Console.WriteLine($"Running warmup {benchmark.Id}");

        for (int i = 0; i < iterationCount; i++)
        {
            benchmark.Method.Invoke(obj, benchmark.Parameters);
        }
    }

    static void RunAllBenchmarks(object obj, List<BenchmarkInfo> benchmarks, int iterationCount, string[,] results)
    {
        Console.WriteLine($"Running {benchmarks.Count} benchmarks");

        int idx = 1;
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in ReflectionHelper.GetAllSetups<NewBenchmarks>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            RunBenchmark(obj, benchmark, iterationCount, results, idx++);
        }
    }

    static void RunBenchmark(object obj, BenchmarkInfo benchmark, int iterationCount, string[,] results, int idx)
    {
        Console.WriteLine($"Running benchmark {benchmark.Id}");

        results[idx, 0] = benchmark.Name;
        results[idx, 1] = benchmark.Parameters?[0].ToString() ?? string.Empty;

        foreach (var metric in _metrics)
            metric.Reset();

        var watch = new Stopwatch();
        for (int i = 0; i < iterationCount; i++)
        {
            watch.Restart();
            BenchmarkResult result = (BenchmarkResult)benchmark.Method.Invoke(obj, benchmark.Parameters);
            watch.Stop();

            foreach (var metric in _metrics)
                metric.HandleResult(result, watch.Elapsed);
        }

        for (int i = 0; i < _metrics.Length; i++)
        {
            results[idx, i + 2] = _metrics[i].FormatMetric();
        }
    }

    public static string BASE_DIRECTORY { get; } = Assembly.GetExecutingAssembly().Location
        .Substring(0, Assembly.GetExecutingAssembly().Location.IndexOf("BlasII.Randomizer.Benchmarks"));
}
