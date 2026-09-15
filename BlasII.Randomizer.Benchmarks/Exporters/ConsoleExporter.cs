
namespace BlasII.Randomizer.Benchmarks.Exporters;

public class ConsoleExporter : IExporter
{
    public void Export(string results)
    {
        Console.WriteLine(results);
    }
}
