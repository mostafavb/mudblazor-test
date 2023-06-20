using MudBlazorTemplates1.Shared.Models;
using MudBlazorTemplates1.WebAssembly.Models;

namespace MudBlazorTemplates1.WebAssembly.Mappers;

public static class OrderMapper
{
    public static OrderDto ToDto(this Order model) =>
        new OrderDto
        {
            CustomerAddress = model.CustomerAddress,
            CustomerName = model.CustomerName,
            Id = model.Id,
            OrderDate = new DateTime(model.OrderDate.Year, model.OrderDate.Month, model.OrderDate.Day),
            OrderTypeId = model.OrderTypeId,
            TotalPrice = model.TotalPrice
        };

    public static List<OrderDto> ToListDto(this IList<Order> models) =>
      models.Select(model =>
        new OrderDto
        {
            CustomerAddress = model.CustomerAddress,
            CustomerName = model.CustomerName,
            Id = model.Id,
            OrderDate = new DateTime(model.OrderDate.Year, model.OrderDate.Month, model.OrderDate.Day),
            OrderTypeId = model.OrderTypeId,
            TotalPrice = model.TotalPrice
        }).ToList();

}
