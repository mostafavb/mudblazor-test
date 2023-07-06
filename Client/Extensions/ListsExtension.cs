using MudBlazorTemplates1.WebAssembly.Utilities;
using MudBlazorTemplates1.WebAssembly.Utils;
using System.Reflection;

namespace MudBlazorTemplates1.WebAssembly.Extensions;

public static class ListsExtension
{
    public static IList<T> GetDataDifference<T>(this IList<T> first, IList<T> second)
    {        
        var propertyInfos = typeof(T).GetProperties().ToList();

        var editableAttributes = propertyInfos?.Where(p =>
        {
            var attributes = p.GetCustomAttribute<DataGridModelAttribute>();
            return attributes is not null && (attributes.IsEditable && !attributes.IsHiden);
        })
                                        .Select(property => property.Name)
                                        .ToArray();

        if (editableAttributes is null)
            return Enumerable.Empty<T>().ToList();

        var comparer = DynamicEqualityComparerFactory.Create<T>(editableAttributes);

        var differences = first.Except(second, comparer);


        if (differences is null)
            return Enumerable.Empty<T>().ToList();

        var diffCounts = differences.Count();


        foreach (var diff in differences)
        {
            Console.WriteLine($"{System.Text.Json.JsonSerializer.Serialize(diff)}");
        }

        return differences.ToList();
    }
}
