using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Metrics;

public class SuccessRateMetric : IMetric<BenchmarkResult>
{
    public string DisplayName { get; } = "Success Rate";

    private int totalAttempts;
    private int successfulAttempts;

    public void HandleResult(BenchmarkResult result, TimeSpan time)
    {
        if (result.WasSuccessful)
            successfulAttempts++;
        totalAttempts++;
    }

    public string FormatMetric()
    {
        if (totalAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double rate = (double)successfulAttempts / totalAttempts * 100;
        return $"{rate:F2} %";
    }

    public void Reset()
    {
        totalAttempts = 0;
        successfulAttempts = 0;
    }
}
