
namespace BlasII.Randomizer.Benchmarks.Metrics;

public interface IMetric<TResult>
{
    public string DisplayName { get; }

    public void HandleResult(TResult result, TimeSpan time);

    public string FormatMetric();

    public void Reset();
}
