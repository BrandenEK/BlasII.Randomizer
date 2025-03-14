using BlasII.Randomizer.Benchmarks.Exporters;
using BlasII.Randomizer.Benchmarks.Metrics;
using BlasII.Randomizer.Benchmarks.Models;
using System.Reflection;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    static void Main(string[] args)
    {
        var cmd = new BenchmarkCommand();
        cmd.Process(args);

        var runner = new BenchmarkRunner<BenchmarkResult>()
            .AddExporter(new ConsoleExporter())
            .AddExporter(cmd.ExportResults, () => new FileExporter(Path.Combine(BASE_DIRECTORY, "BenchmarkResults")))
            .AddMetrics(new SuccessRateMetric(), new AverageTimeMetric(), new AverageSuccessTimeMetric());

        runner.Run<NewBenchmarks>(cmd.MaxIterations);

        if (cmd.WaitForInput)
            Console.ReadKey(true);
    }

    public static string BASE_DIRECTORY { get; } = Assembly.GetExecutingAssembly().Location
        .Substring(0, Assembly.GetExecutingAssembly().Location.IndexOf("BlasII.Randomizer.Benchmarks"));
}
