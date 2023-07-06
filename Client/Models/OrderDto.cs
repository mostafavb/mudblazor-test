using MudBlazorTemplates1.Shared.Models;
using MudBlazorTemplates1.WebAssembly.Utilities;

namespace MudBlazorTemplates1.WebAssembly.Models;

public class OrderDto
{
    [DataGridModel(IsEditable = false, IsReadOnly = true, IsSortable = true, IsFilterable = false, Title = "Id")]
    public int Id { get; set; }

    [DataGridModel(IsEditable = true, IsFilterable = true, IsSortable = true, Title = "Date", Format = "yyyy/MM/dd")]
    public DateTime? OrderDate { get; set; }

    [DataGridModel(IsEditable = true, IsSortable = true, IsFilterable = true, Title = "Customer Name")]
    public string? CustomerName { get; set; }

    [DataGridModel(IsHiden = true)]
    public string? CustomerAddress { get; set; }

    [DataGridModel(IsHiden = true)]
    public int OrderTypeId { get; set; }

    [DataGridModel(IsSortable = true, IsFilterable = true,IsEditable =true, Title = "Total Price")]
    public decimal TotalPrice { get; set; }

    [DataGridModel(IsHiden = true,IsEditable = true, Title = "Ordered")]
    public bool IsOrder { get; set; } = true;
}
