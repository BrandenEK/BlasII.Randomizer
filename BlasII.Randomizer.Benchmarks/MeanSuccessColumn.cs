using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

public class MeanSuccessColumn : IColumn
{
    public string Id { get; } = nameof(MeanSuccessColumn);

    public string ColumnName { get; } = "Mean (Successful)";

    public bool AlwaysShow { get; } = true;

    public ColumnCategory Category { get; } = ColumnCategory.Custom;

    public int PriorityInCategory { get; } = 0;

    public bool IsNumeric { get; } = true;

    public UnitType UnitType { get; } = UnitType.Time;

    public string Legend { get; } = "Average time of successful attempts";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        return "0 ms";
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return GetValue(summary, benchmarkCase);
    }

    public bool IsAvailable(Summary summary)
    {
        return true;
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
    {
        return true;
    }
}
