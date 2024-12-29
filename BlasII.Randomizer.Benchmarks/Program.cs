using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        //var config = new ManualConfig();
        //config.AddColumn(new TargetMethodColumn(), new SuccessRateColumn());
        //config.AddLogger(new ConsoleLogger());

        var summary = BenchmarkRunner.Run<ShufflerBenchmarks>();
    }
}
