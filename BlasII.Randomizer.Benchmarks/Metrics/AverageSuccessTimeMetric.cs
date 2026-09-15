using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Metrics;

public class AverageSuccessTimeMetric : IMetric<BenchmarkResult>
{
    public string DisplayName { get; } = "Avg. Time (Successful)";

    private TimeSpan successfulTime;
    private int successfulAttempts;

    public void HandleResult(BenchmarkResult result, TimeSpan time)
    {
        if (result.WasSuccessful)
        {
            successfulTime += time;
            successfulAttempts++;
        }
    }

    public string FormatMetric()
    {
        if (successfulAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double time = successfulTime.TotalMilliseconds / successfulAttempts;
        return $"{time:F2} ms";
    }

    public void Reset()
    {
        successfulTime = TimeSpan.Zero;
        successfulAttempts = 0;
    }
}
