using BlasII.Randomizer.Benchmarks.Attributes;
using System.Reflection;

namespace BlasII.Randomizer.Benchmarks;

internal static class ReflectionHelper
{
    public static IEnumerable<object> GetParameters<TClass>(object obj, string name)
    {
        PropertyInfo property = typeof(TClass).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        return property == null
            ? throw new Exception($"Failed to find property with the name {name}")
            : (IEnumerable<object>)property.GetValue(obj);
    }

    public static IEnumerable<MethodInfo> GetAllSetups<TClass>(string target) where TClass : class
    {
        return typeof(TClass).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => HasValidSetupAttribute(x, target));
    }

    private static bool HasValidSetupAttribute(MethodInfo method, string target)
    {
        BenchmarkSetupAttribute attribute = method.GetCustomAttribute<BenchmarkSetupAttribute>();
        return attribute != null && (string.IsNullOrEmpty(attribute.Target) || attribute.Target == target);
    }
}
