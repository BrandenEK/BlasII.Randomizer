
namespace BlasII.Randomizer.Benchmarks;

public abstract class BaseMonitor
{
    private readonly Dictionary<string, MonitorStatus> _statuses = new();

    public abstract string DisplayName { get; }

    public void HandleResult(string name, TimeSpan time, bool result)
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

    protected abstract void HandleResult(MonitorStatus status, TimeSpan time, bool result);

    protected abstract string FormatResult(MonitorStatus status);
}

public class SuccessRateMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Success Rate";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, bool result)
    {
        if (result)
            status.SuccessfulAttempts++;
        status.TotalAttempts++;
    }

    protected override string FormatResult(MonitorStatus status)
    {
        if (status.TotalAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double rate = (double)status.SuccessfulAttempts / status.TotalAttempts * 100;
        return $"{rate:F2}%";
    }
}

public class AverageTimeMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Avg. Time (All)";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, bool result)
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

public class AverageSuccessTimeMonitor : BaseMonitor
{
    public override string DisplayName { get; } = "Avg. Time (Successful)";

    protected override void HandleResult(MonitorStatus status, TimeSpan time, bool result)
    {
        if (result)
        {
            status.SuccessfulTime+= time;
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

public class MonitorStatus
{
    public int SuccessfulAttempts { get; set; } = 0;
    public int TotalAttempts { get; set; } = 0;

    public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
    public TimeSpan SuccessfulTime { get; set; } = TimeSpan.Zero;
}
