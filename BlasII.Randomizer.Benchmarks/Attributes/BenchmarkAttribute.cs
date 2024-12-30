
namespace BlasII.Randomizer.Benchmarks.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BenchmarkAttribute : Attribute
{
    internal string Name { get; }

    public BenchmarkAttribute(string name)
    {
        Name = name;
    }

    public BenchmarkAttribute()
    {
        Name = null;
    }
}
