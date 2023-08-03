using static MudBlazor.CategoryTypes;
using System.Reflection;
using Ui.WebAssembly.Extensions;

namespace Ui.WebAssembly.Utilities;

public static class PropertyInfoHelpers
{
    /// <summary>
    /// return a list of PropertyInfo for all properties in a type of T
    /// </summary>
    /// <param name="item"></param>
    /// <returns>List<PropertyInfo></returns>
    public static List<PropertyInfo> GetPropertyInfos<T>(T item) =>
        item
            .GetType()
            .GetProperties()
            .ToList();


    /// <summary>
    /// Log some information about the attributes of a property
    /// </summary>
    public static void LogTypeAttributes(List<PropertyInfo> propertyInfos)
    {
        foreach (var prop in propertyInfos)
        {
            var itemName = prop.Name;
            Console.WriteLine($"itemName: {itemName}");

            bool isReadOnly = prop.GetAttributeValue<bool>("IsReadOnly");
            Console.WriteLine($"\tIsReadOnly: {isReadOnly}");

            bool isHiden = prop.GetAttributeValue<bool>("IsHiden");
            Console.WriteLine($"\tIsHiden: {isHiden}");

            bool isEditable = prop.GetAttributeValue<bool>("IsEditable");
            Console.WriteLine($"\tIsEditable: {isEditable}");

            bool isFilterable = prop.GetAttributeValue<bool>("IsFilterable");
            Console.WriteLine($"\tIsFilterable: {isFilterable}");

            bool isSortable = prop.GetAttributeValue<bool>("IsSortable");
            Console.WriteLine($"\tIsSortable: {isSortable}");

            string title = prop.GetAttributeValue<string>("Title") ?? string.Empty;
            Console.WriteLine($"\tTitle: {title}");

            string placeholder = prop.GetAttributeValue<string>("Placeholder") ?? string.Empty;
            Console.WriteLine($"\tPlaceholder: {placeholder}");

            string format = prop.GetAttributeValue<string>("Format") ?? string.Empty;
            Console.WriteLine($"\tFormat: {format}");

            string context = prop.GetAttributeValue<string>("Context") ?? string.Empty;
            Console.WriteLine($"\tContext: {context}");

            string cellClass = prop.GetAttributeValue<string>("CellClass") ?? string.Empty;
            Console.WriteLine($"\tCellClass: {cellClass}");

            string cellStyle = prop.GetAttributeValue<string>("CellStyle") ?? string.Empty;
            Console.WriteLine($"\tCellStyle: {cellStyle}");

            string class_ = prop.GetAttributeValue<string>("Class") ?? string.Empty;
            Console.WriteLine($"\tClass: {class_}");


        }
    }

    public static void LogAttributes<T>(PropertyInfo prop,T tItem)
    {
        var attributes = prop.GetCustomAttribute<GenericDataGridPrefrencesAttribute>();

        Console.WriteLine($"item: Name: {prop.Name}");
        Console.WriteLine($"\tType: {prop.PropertyType}");
        Console.WriteLine($"\tValue: {prop.GetValue(tItem)}");

        bool isDatetime = prop.PropertyType == typeof(DateTime?);
        Console.WriteLine($"\tIsDatetime: {isDatetime}");
        Console.WriteLine($"\tIsEditable: {attributes.IsEditable}");
        Console.WriteLine($"\tIsSortable: {attributes.IsSortable}");
        Console.WriteLine($"\tIsHiden: {attributes.IsHiden}");
        Console.WriteLine($"\tIsFilterable: {attributes.IsFilterable}");
        Console.WriteLine($"----------------------------------");
    }
}
