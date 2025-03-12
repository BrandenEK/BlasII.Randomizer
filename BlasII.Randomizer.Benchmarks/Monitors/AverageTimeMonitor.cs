using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Monitors;

public class AverageTimeMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Avg. Time (All)";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, BenchmarkResult result)
    {
        status.TotalTime += time;
        status.TotalAttempts++;
    }

    protected override string FormatResult(MonitorStatus status)
    {
        if (status.TotalAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double time = status.TotalTime.TotalMilliseconds / status.TotalAttempts;
        return $"{time:F2} ms";
    }
}
