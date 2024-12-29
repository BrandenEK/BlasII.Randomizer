using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BlasII.Randomizer.Benchmarks;

public class SuccessRateColumn : IColumn
{
    public string Id { get; } = nameof(SuccessRateColumn);

    public string ColumnName { get; } = "Success Rate";

    public bool AlwaysShow { get; } = true;

    public ColumnCategory Category { get; } = ColumnCategory.Custom;

    public int PriorityInCategory { get; } = 0;

    public bool IsNumeric { get; } = true;

    public UnitType UnitType { get; } = UnitType.Dimensionless;

    public string Legend { get; } = "Percent of successful shuffles";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        return "Unknown";
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
