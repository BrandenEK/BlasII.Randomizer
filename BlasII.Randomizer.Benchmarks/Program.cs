using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

internal class Program
{
    private static List<IResultHandler> _columns = new();

    static void Main(string[] args)
    {
        var timeall = new ColumnTimeAll();
        var timesuccess = new ColumnTimeSuccess();
        var successrate = new ColumnSuccessRate();

        //_columns = new List<IResultHandler>()
        //{
        //    successrate,
        //};
        var cols = new List<IColumn>() // temp
        {
            timeall,
            timesuccess,
            successrate,
        };

        var config = new ManualConfig();
        config.AddColumn(TargetMethodColumn.Method);
        config.AddColumn(cols.ToArray());
        config.AddLogger(ConsoleLogger.Default);
        config.AddExporter(MarkdownExporter.GitHub);

        var summary = BenchmarkRunner.Run<ShufflerBenchmarks>(config);
    }

    //public static void RegisterColumn(IResultHandler handler)
    //{
    //    _columns.Add(handler);
    //}

    //public static void HandleResult(ShuffleResult result)
    //{
    //    //Console.WriteLine($"{result.Name} has a result of {(result.Successful ? "Success" : "Fail")}");
    //    foreach (var column in _columns)
    //        column.HandleResult(result);
    //}
}
