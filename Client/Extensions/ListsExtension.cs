using Ui.WebAssembly.Utilities;
using System.Reflection;
using MiniExcelLibs.Attributes;
using System.Linq;

namespace Ui.WebAssembly.Extensions;

internal static class ListsExtension
{
    public static IList<T> FindDifferences<T>(this IList<T> first, IList<T> second, bool checkFile = false)
    {
        var propertyInfos = typeof(T).GetProperties().ToList();

        var editableProperties = propertyInfos?.Where(p =>
        {
            var attributes = p.GetCustomAttribute<GenericDataGridPrefrencesAttribute>();
            bool ignoreExcel = false;
            if (checkFile)
            {
                var excelAttributes = p.GetCustomAttribute<ExcelColumnAttribute>();
                ignoreExcel = excelAttributes is not null && excelAttributes.Ignore;
            }
            return attributes is not null && (attributes.IsEditable && !attributes.IsHiden) && !ignoreExcel;
        }).ToList();


        if (editableProperties is null)
            return Enumerable.Empty<T>().ToList();

        var comparer = DynamicEqualityComparerFactory.Create<T>(editableProperties);

        var differences = first.Except(second, comparer);

        return (differences ?? Enumerable.Empty<T>()).ToList();
    }

    public static IList<T> FindDifferencesByKeies<T>(this IList<T> first, IList<T> second)
    {
        if (!(first?.Count > 0 || second?.Count > 0))
            return Enumerable.Empty<T>().ToList();

        var quailifiedProperties = first.First()?.GetType()
                                    .GetProperties()
                                    .Where(p =>
                                    {
                                        var dataGridAttrs = p.GetCustomAttribute<GenericDataGridPrefrencesAttribute>();
                                        return (dataGridAttrs is not null && (dataGridAttrs.IsKey));
                                    }).ToList();
        if (quailifiedProperties?.Count > 0)
        {            
            var comparer = DynamicEqualityComparerFactory.Create<T>(quailifiedProperties);

            return (first.Except(second, comparer) ?? Enumerable.Empty<T>()).ToList();
        }
        return (Enumerable.Empty<T>()).ToList();
    }

    public static void UpdateProperties<T>(List<T> destinationList, List<T> sourceList)
    {
        if (destinationList.Count != sourceList.Count)
        {
            throw new ArgumentException("Both lists must have the same number of elements.");
        }

        PropertyInfo[] destinationProperties = typeof(T).GetProperties();

        foreach (T sourceItem in sourceList)
        {
            T destinationItem = destinationList[sourceList.IndexOf(sourceItem)];
            PropertyInfo[] sourceProperties = sourceItem.GetType().GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = destinationProperties.FirstOrDefault(prop => prop.Name == sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    if (destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        destinationProperty.SetValue(destinationItem, sourceProperty.GetValue(sourceItem));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Property types do not match for '{destinationProperty.Name}'.");
                    }
                }
            }
        }
    }

    private static IList<Dictionary<string, object?>> GetKeyedProperties<T>(IList<T> list)
    {
     var lst =   list.Select(item => new
        {
            quailifiedProperties = item.GetType()
                                    .GetProperties()
                                    .Where(p =>
                                    {
                                        var dataGridAttrs = p.GetCustomAttribute<GenericDataGridPrefrencesAttribute>();
                                        return (dataGridAttrs is not null && (dataGridAttrs.IsKey));
                                    })
                                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(item))
        })
        .Select(s => s.quailifiedProperties)
        .ToList();
        
        return lst;
    }

}
