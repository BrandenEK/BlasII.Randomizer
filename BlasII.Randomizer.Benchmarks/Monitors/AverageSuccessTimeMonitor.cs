using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Monitors;

public class AverageSuccessTimeMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Avg. Time (Successful)";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, BenchmarkResult result)
    {
        if (result.WasSuccessful)
        {
            status.SuccessfulTime += time;
            status.SuccessfulAttempts++;
        }
    }

    protected override string FormatResult(MonitorStatus status)
    {
        if (status.SuccessfulAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double time = status.SuccessfulTime.TotalMilliseconds / status.SuccessfulAttempts;
        return $"{time:F2} ms";
    }
}
