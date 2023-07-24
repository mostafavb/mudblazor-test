using System.Reflection;

namespace Ui.WebAssembly.Utils;

public class DynamicEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> equalsFunc;
    private readonly Func<T, int> hashCodeFunc;

    public DynamicEqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> hashCodeFunc)
    {
        this.equalsFunc = equalsFunc;
        this.hashCodeFunc = hashCodeFunc;
    }

    public bool Equals(T x, T y) => equalsFunc(x, y);
    public int GetHashCode(T obj) => hashCodeFunc(obj);
}

public static class DynamicEqualityComparerFactory
{
    public static IEqualityComparer<T> Create<T>(params string[] propertyNames)
    {
        var type = typeof(T);
        var properties = new List<PropertyInfo>();

        foreach (var propertyName in propertyNames)
        {
            var property = type.GetProperty(propertyName);
            if (property != null)
                properties.Add(property);
        }

        return new DynamicEqualityComparer<T>(
            (x, y) =>
            {
                foreach (var property in properties)
                {
                    var xValue = property.GetValue(x);
                    var yValue = property.GetValue(y);
                    if (!Equals(xValue, yValue))
                        return false;
                }
                return true;
            },
            obj =>
            {
                unchecked
                {
                    int hash = 17;
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(obj);
                        if (value != null)
                            hash = hash * 23 + value.GetHashCode();
                    }
                    return hash;
                }
            });
    }

    public static IEqualityComparer<T> Create<T>(List<PropertyInfo> properties)
    {
        return new DynamicEqualityComparer<T>(
            (x, y) =>
            {
                foreach (var property in properties)
                {
                    var xValue = property.GetValue(x);
                    var yValue = property.GetValue(y);
                    if (!Equals(xValue, yValue))
                        return false;
                }
                return true;
            },
            obj =>
            {
                unchecked
                {
                    int hash = 17;
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(obj);
                        if (value != null)
                            hash = hash * 23 + value.GetHashCode();
                    }
                    return hash;
                }
            });
    }
}