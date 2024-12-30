
namespace BlasII.Randomizer.Benchmarks.Models;

public class ParameterInfo
{
    public string Name { get; }

    public object Value { get; }

    public ParameterInfo(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
