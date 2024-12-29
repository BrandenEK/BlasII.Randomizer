using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

public class ColumnSuccessRate : IColumn
{
    private static readonly Dictionary<string, SuccessRateStatus> _statuses = new();

    public string Id { get; } = nameof(ColumnSuccessRate);

    public string ColumnName { get; } = "Success Rate";

    public bool AlwaysShow { get; } = true;

    public ColumnCategory Category { get; } = ColumnCategory.Custom;

    public int PriorityInCategory { get; } = 0;

    public bool IsNumeric { get; } = true;

    public UnitType UnitType { get; } = UnitType.Dimensionless;

    public string Legend { get; } = "Percent of successful attempts";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        foreach (var kvp in _statuses)
        {
            Console.WriteLine(kvp.Key);
            Console.WriteLine("Total: " + kvp.Value.AttemptsTotal);
        }

        if (!_statuses.TryGetValue(benchmarkCase.DisplayInfo, out SuccessRateStatus status))
            throw new Exception($"Benchmark status {benchmarkCase.DisplayInfo} was not found");

        if (status.AttemptsTotal < 1)
            throw new Exception($"There were 0 total attempts");

        float result = (float)status.AttemptsSuccessful / status.AttemptsTotal * 100;
        return $"{result:F2}%";
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return GetValue(summary, benchmarkCase);
    }

    public static void HandleResult(ShuffleResult result)
    {
        if (!_statuses.TryGetValue(result.Name, out SuccessRateStatus status))
        {
            Console.WriteLine("Adding status for " +  result.Name);
            status = new SuccessRateStatus();
            _statuses.Add(result.Name, status);
        }

        if (result.Successful)
            status.AttemptsSuccessful++;
        status.AttemptsTotal++;
    }

    public bool IsAvailable(Summary summary)
    {
        return true;
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
    {
        return true;
    }

    class SuccessRateStatus
    {
        public int AttemptsSuccessful { get; set; } = 0;
        public int AttemptsTotal { get; set; } = 0;
    }
}
