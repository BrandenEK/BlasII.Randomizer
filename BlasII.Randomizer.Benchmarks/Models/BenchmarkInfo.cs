using System.Reflection;

namespace BlasII.Randomizer.Benchmarks.Models;

public class BenchmarkInfo
{
    public string Id { get; }

    public string Name { get; }

    public MethodInfo Method { get; }

    public object[] Parameters { get; }

    public BenchmarkInfo(string id, string name, MethodInfo method, object[] parameters)
    {
        Id = id;
        Name = name;
        Method = method;
        Parameters = parameters;
    }
}
