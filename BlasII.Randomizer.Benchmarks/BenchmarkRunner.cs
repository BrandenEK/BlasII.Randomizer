using BlasII.Randomizer.Benchmarks.Exporters;

namespace BlasII.Randomizer.Benchmarks;

public class BenchmarkRunner
{
    private static readonly List<IExporter> _exporters = new();

    public BenchmarkRunner AddExporter(IExporter exporter)
    {
        _exporters.Add(exporter);
        return this;
    }

    public BenchmarkRunner AddExporter(bool condition, Func<IExporter> createExporter)
    {
        if (condition)
            _exporters.Add(createExporter());
        return this;
    }

    public BenchmarkRunner AddExporters(params IExporter[] exporters)
    {
        _exporters.AddRange(exporters);
        return this;
    }

    public BenchmarkRunner AddExporters(bool condition, params Func<IExporter>[] createExporters)
    {
        if (condition)
            _exporters.AddRange(createExporters.Select(x => x.Invoke()));
        return this;
    }

    public void Run(int iterations)
    {
        // Do the actual stuff
    }
}
