using BlasII.Randomizer.Benchmarks.Models;

namespace BlasII.Randomizer.Benchmarks.Monitors;

public abstract class BaseMonitor
{
    private readonly Dictionary<string, MonitorStatus> _statuses = new();

    public abstract string DisplayName { get; }

    public void HandleResult(string name, TimeSpan time, BenchmarkResult result)
    {
        if (!_statuses.TryGetValue(name, out MonitorStatus status))
        {
            status = new MonitorStatus();
            _statuses.Add(name, status);
        }

        HandleResult(status, time, result);
    }

    public string FormatResult(string name)
    {
        if (!_statuses.TryGetValue(name, out MonitorStatus status))
            throw new Exception($"Benchmark status {name} was not found");

        return FormatResult(status);
    }

    protected abstract void HandleResult(MonitorStatus status, TimeSpan time, BenchmarkResult result);

    protected abstract string FormatResult(MonitorStatus status);
}

public class MonitorStatus
{
    public int SuccessfulAttempts { get; set; } = 0;
    public int TotalAttempts { get; set; } = 0;

    public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
    public TimeSpan SuccessfulTime { get; set; } = TimeSpan.Zero;
}
