namespace Infrastructure.Shared.Models;
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

    public int FactorId { get; set; }
    public string? VisitorName { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Payment { get; set; }
    public int PaymentId { get; set; }
    public decimal LastPayment { get; set; }
    public decimal Remains { get; set; }
    public bool IsClosed { get; set; }
    public bool HasAccounting { get; set; }
    public string? BankAccountingId { get; set; }
    public DateOnly PaymentDate { get; set; }

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
