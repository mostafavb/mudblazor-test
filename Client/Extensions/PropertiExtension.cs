using System.Reflection;

namespace MudBlazorTemplates1.WebAssembly.Extensions;

public static class PropertiExtension
{
    public static TReturn GetAttributeValue<TReturn>(this PropertyInfo property, object obj) =>
        (TReturn)property.GetValue(obj)!;       

}
