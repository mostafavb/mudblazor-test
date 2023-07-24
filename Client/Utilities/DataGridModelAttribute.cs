namespace Ui.WebAssembly.Utilities;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DataGridModelAttribute : Attribute
{
    public bool IsReadOnly { get; set; } = false;
    public bool IsHiden { get; set; } = false;
    public bool IsEditable { get; set; } = false;
    public bool IsFilterable { get; set; } = false;
    public bool IsSortable { get; set; } = false;
    public Type? DataType { get; set; } = null;    
    public string Title { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
    public string CellClass { get; set; } = string.Empty;
    public string CellStyle { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;

}
