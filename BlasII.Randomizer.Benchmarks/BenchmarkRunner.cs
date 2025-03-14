using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Exporters;
using BlasII.Randomizer.Benchmarks.Metrics;
using BlasII.Randomizer.Benchmarks.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BlasII.Randomizer.Benchmarks;

public class BenchmarkRunner<TResult>
{
    private static readonly List<IExporter> _exporters = new();
    private static readonly List<IMetric<TResult>> _metrics = new();

    public void Run<TClass>(int iterations, bool doWarmup) where TClass : class, new()
    {
        object obj = new TClass();
        var benchmarks = FindAllBenchmarks<TClass>(obj);

        if (doWarmup)
            RunAllWarmups<TClass>(obj, benchmarks, iterations / 5);

        var results = new string[benchmarks.Count + 1, _metrics.Count + 2];
        RunAllBenchmarks<TClass>(obj, benchmarks, iterations, results);

        string display = GetInfoDisplay(iterations) + GetResultsDisplay(results);

        foreach (var exporter in _exporters)
            exporter.Export(display);
    }

    // Finding benchmarks

    private List<BenchmarkInfo> FindAllBenchmarks<TClass>(object obj)
    {
        var benchmarks = new List<BenchmarkInfo>();
        var methods = typeof(TClass).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

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
                ? ReflectionHelper.GetParameters<TClass>(obj, bpAttribute.ParameterProperty)
                : bpAttribute.Parameters ?? throw new Exception("You have to add the parameter property or the list");

            benchmarks.AddRange(parameters
                .Select(x => new BenchmarkInfo($"{method.Name}.{x}", bAttribute.Name ?? method.Name, method, new object[] { x })));
        }

        return benchmarks;
    }

    // Running warmups

    private void RunAllWarmups<TClass>(object obj, List<BenchmarkInfo> benchmarks, int iterationCount) where TClass : class
    {
        Console.WriteLine($"Running {benchmarks.Count} warmups");
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in ReflectionHelper.GetAllSetups<TClass>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            RunWarmup(obj, benchmark, iterationCount);
        }
    }

    private void RunWarmup(object obj, BenchmarkInfo benchmark, int iterationCount)
    {
        Console.WriteLine($"Running warmup {benchmark.Id}");

        for (int i = 0; i < iterationCount; i++)
        {
            benchmark.Method.Invoke(obj, benchmark.Parameters);
        }
    }

    // Running benchmarks

    private void RunAllBenchmarks<TClass>(object obj, List<BenchmarkInfo> benchmarks, int iterationCount, string[,] results) where TClass : class
    {
        Console.WriteLine($"Running {benchmarks.Count} benchmarks");

        int idx = 1;
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in ReflectionHelper.GetAllSetups<TClass>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            RunBenchmark(obj, benchmark, iterationCount, results, idx++);
        }
    }

    private void RunBenchmark(object obj, BenchmarkInfo benchmark, int iterationCount, string[,] results, int idx)
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
            TResult result = (TResult)benchmark.Method.Invoke(obj, benchmark.Parameters);
            watch.Stop();

            foreach (var metric in _metrics)
                metric.HandleResult(result, watch.Elapsed);
        }

        for (int i = 0; i < _metrics.Count; i++)
        {
            results[idx, i + 2] = _metrics[i].FormatMetric();
        }
    }

    // Calculating display

    private string GetInfoDisplay(int iterationCount)
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

    private string GetResultsDisplay(string[,] results)
    {
        var sb = new StringBuilder();

        // Fill header row
        results[0, 0] = "Method";
        results[0, 1] = "Parameters";
        for (int i = 0; i < _metrics.Count; i++)
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

    // Adding exporters

    public BenchmarkRunner<TResult> AddExporter(IExporter exporter)
    {
        _exporters.Add(exporter);
        return this;
    }

    public BenchmarkRunner<TResult> AddExporter(bool condition, Func<IExporter> createExporter)
    {
        if (condition)
            _exporters.Add(createExporter());
        return this;
    }

    public BenchmarkRunner<TResult> AddExporters(params IExporter[] exporters)
    {
        _exporters.AddRange(exporters);
        return this;
    }

    public BenchmarkRunner<TResult> AddExporters(bool condition, params Func<IExporter>[] createExporters)
    {
        if (condition)
            _exporters.AddRange(createExporters.Select(x => x.Invoke()));
        return this;
    }

    // Adding metrics

    public BenchmarkRunner<TResult> AddMetric(IMetric<TResult> metric)
    {
        _metrics.Add(metric);
        return this;
    }

    public BenchmarkRunner<TResult> AddMetric(bool condition, Func<IMetric<TResult>> createMetric)
    {
        if (condition)
            _metrics.Add(createMetric());
        return this;
    }

    public BenchmarkRunner<TResult> AddMetrics(params IMetric<TResult>[] metrics)
    {
        _metrics.AddRange(metrics);
        return this;
    }

    public BenchmarkRunner<TResult> AddMetrics(bool condition, Func<IMetric<TResult>>[] createMetrics)
    {
        if (condition)
            _metrics.AddRange(createMetrics.Select(x => x.Invoke()));
        return this;
    }
}
