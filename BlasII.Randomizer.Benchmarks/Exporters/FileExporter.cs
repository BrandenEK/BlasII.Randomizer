
namespace BlasII.Randomizer.Benchmarks.Exporters;

public class FileExporter : IExporter
{
    private readonly string _directory;

    public FileExporter(string directory)
    {
        _directory = directory;
    }

    public void Export(string results)
    {
        try
        {
            Directory.CreateDirectory(_directory);
            File.WriteAllText(Path.Combine(_directory, "Latest.txt"), results);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export results to {_directory}: {ex}");
        }
    }
}
