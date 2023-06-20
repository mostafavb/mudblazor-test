using MudBlazorTemplates1.Shared.Models;

namespace MudBlazorTemplates1.WebAssembly.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public DateTime? OrderDate { get; set; }
    public int OrderTypeId { get; set; }
    public decimal TotalPrice { get; set; }
}
