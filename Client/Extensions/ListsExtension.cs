using Ui.WebAssembly.Utilities;
using Ui.WebAssembly.Utils;
using System.Reflection;
using MiniExcelLibs.Attributes;

namespace Ui.WebAssembly.Extensions;

public static class ListsExtension
{    
    public static IList<T> GetDataDifference<T>(this IList<T> first, IList<T> second, bool checkFile = false)
    {
        var propertyInfos = typeof(T).GetProperties().ToList();

        var editableProperties = propertyInfos?.Where(p =>
        {
            var attributes = p.GetCustomAttribute<DataGridModelAttribute>();
            bool ignoreExcel = false;
            if (checkFile)
            {
                var excelAttributes = p.GetCustomAttribute<ExcelColumnAttribute>();
                ignoreExcel = excelAttributes is not null && excelAttributes.Ignore;
            }
            return attributes is not null && (attributes.IsEditable && !attributes.IsHiden) && !ignoreExcel;
        }).ToList();
        //.Select(property => property.Name)
        //.ToArray();

        if (editableProperties is null)
            return Enumerable.Empty<T>().ToList();

        var comparer = DynamicEqualityComparerFactory.Create<T>(editableProperties);

        var differences = first.Except(second, comparer);


        if (differences is null)
            return Enumerable.Empty<T>().ToList();

        //var diffCounts = differences.Count();

        //foreach (var diff in differences)
        //{
        //    Console.WriteLine($"{System.Text.Json.JsonSerializer.Serialize(diff)}");
        //}

        return differences.ToList();
    }
}
