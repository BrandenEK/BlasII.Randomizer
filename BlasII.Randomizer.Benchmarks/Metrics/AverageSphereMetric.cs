using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Metrics;

public class AverageSphereMetric : IMetric<BenchmarkResult>
{
    public string DisplayName { get; } = "Avg. Sphere Size";

    private int successfulSpheres;
    private int successfulAttempts;

    public void HandleResult(BenchmarkResult result, TimeSpan time)
    {
        if (!result.WasSuccessful)
            return;

        successfulSpheres += 10;
        successfulAttempts++;
    }

    public string FormatMetric()
    {
        if (successfulAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double value = (double)successfulSpheres / successfulAttempts;
        return $"{value:F1}";
    }

    public void Reset()
    {
        successfulSpheres = 0;
        successfulAttempts = 0;
    }
}
