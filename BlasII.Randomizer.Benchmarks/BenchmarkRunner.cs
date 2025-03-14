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

    public BenchmarkRunner AddExporter(IExporter exporter, bool condition)
    {
        if (condition)
            _exporters.Add(exporter);
        return this;
    }

    public BenchmarkRunner AddExporters(params IExporter[] exporters)
    {
        _exporters.AddRange(exporters);
        return this;
    }

    public void Run(int iterations)
    {
        // Do the actual stuff
    }
}
