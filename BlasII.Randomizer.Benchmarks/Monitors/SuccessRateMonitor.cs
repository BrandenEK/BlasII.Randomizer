using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Monitors;

public class SuccessRateMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Success Rate";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, BenchmarkResult result)
    {
        if (result.WasSuccessful)
            status.SuccessfulAttempts++;
        status.TotalAttempts++;
    }

    protected override string FormatResult(MonitorStatus status)
    {
        if (status.TotalAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double rate = (double)status.SuccessfulAttempts / status.TotalAttempts * 100;
        return $"{rate:F2} %";
    }
}
