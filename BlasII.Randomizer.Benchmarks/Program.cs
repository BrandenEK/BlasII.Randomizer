using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        var config = new ManualConfig();
        config.AddColumn(TargetMethodColumn.Method, new ColumnTimeAll(), new ColumnTimeSuccess(), new ColumnSuccessRate());
        config.AddLogger(ConsoleLogger.Default);
        config.AddExporter(MarkdownExporter.GitHub);

        var summary = BenchmarkRunner.Run<ShufflerBenchmarks>(config);
    }
}
