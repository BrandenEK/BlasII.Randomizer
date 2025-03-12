using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Metrics;

public class AverageTimeMetric : IMetric<BenchmarkResult>
{
    public string DisplayName { get; } = "Avg. Time (All)";

    private TimeSpan totalTime;
    private int totalAttempts;

    public void HandleResult(BenchmarkResult result, TimeSpan time)
    {
        totalTime += time;
        totalAttempts++;
    }

    public string FormatMetric()
    {
        if (totalAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double time = totalTime.TotalMilliseconds / totalAttempts;
        return $"{time:F2} ms";
    }

    public void Reset()
    {
        totalTime = TimeSpan.Zero;
        totalAttempts = 0;
    }
}
