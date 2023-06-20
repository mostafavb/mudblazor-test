namespace MudBlazorTemplates1.Shared.Models;
public class Order
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public DateOnly OrderDate { get; set; }
    public int OrderTypeId { get; set; }
    public OrderType OrderType { get; set; }

    public List<OrderDetail>? OrderDetails { get; set; }

    public decimal TotalPrice => OrderDetails.Sum(s => s.Price);
}

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal Amount { get; set; }
    public Product Product { get; set; }
    public decimal Price => Amount * Product.UnitPrice;
}

public record OrderType(int Id, string Name);
public record Product(int Id, string Name, decimal UnitPrice);
