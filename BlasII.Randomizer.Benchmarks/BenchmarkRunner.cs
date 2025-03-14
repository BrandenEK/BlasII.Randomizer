using BlasII.Randomizer.Benchmarks.Exporters;
using BlasII.Randomizer.Benchmarks.Metrics;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BlasII.Randomizer.Benchmarks;

public class BenchmarkRunner<TResult>
{
    private static readonly List<IExporter> _exporters = new();
    private static readonly List<IMetric<TResult>> _metrics = new();

    public void Run(int iterations)
    {
        // Do the actual stuff
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
}
