using Bogus;
using MudBlazorTemplates1.Shared.Models;

namespace ApiClient.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly List<Order> _orders;
    private readonly FakeDataGenerator _fakeDataGenerator;

    public OrdersRepository()
    {
        _fakeDataGenerator = new();

        _orders = _fakeDataGenerator.GenerateFakeOrders(10);
    }

    public List<Order> GetAllOrders()
    {
        return _orders;
    }

    public Order? GetOrderById(int id)
    {
        return _orders.FirstOrDefault(o => o.Id == id);
    }

    public void AddOrder(Order order)
    {
        order.Id = _orders.Count + 1;
        _orders.Add(order);
    }

    public void DeleteOrder(Order order)
    {
        _orders.Remove(order);
    }

    public List<OrderType> GetOrderTypes() =>
        _fakeDataGenerator.OrderTypes;
    
    public List<Product> GetProducts()=>
        _fakeDataGenerator.Products;
}

public class FakeDataGenerator
{
    public List<OrderType> OrderTypes = new List<OrderType>()
        {
            new OrderType(1, "Online"),
            new OrderType(2, "In store"),
            new OrderType(3, "Warehouse")
        };

    public List<Product> Products = new List<Product>()
        {
            new Product(1, "Product 1", 10.0m),
            new Product(2, "Product 2", 15.0m),
            new Product(3, "Product 3", 20.0m),
            new Product(4, "Product 4", 25.0m)
        };

    public List<Order> GenerateFakeOrders(int count)
    {
        var faker = new Faker();

        var orders = new List<Order>();

        for (int i = 0; i < count; i++)
        {
            int otId = faker.PickRandom(OrderTypes).Id;
            int id = 0;
            do
            {
                id = faker.Random.Int(0, count*10);
            }
            while (orders.Any(a => a.Id == id));


            var order = new Order()
            {

                Id = id,
                CustomerName = faker.Name.FullName(),
                CustomerAddress = faker.Address.FullAddress(),
                OrderDate = faker.Date.PastDateOnly(),
                OrderTypeId = otId,
                OrderDetails = new List<OrderDetail>(),
                OrderType = OrderTypes.FirstOrDefault(f => f.Id == otId)
            };

            var numOrderDetails = faker.Random.Int(1, 5);

            for (int j = 0; j < numOrderDetails; j++)
            {
                var product = faker.PickRandom(Products);

                var orderDetail = new OrderDetail()
                {
                    Id = faker.Random.Int(1, 100),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Amount = Math.Round(faker.Random.Decimal(1.0m, 10.0m), 2),
                    Product = product
                };

                order.OrderDetails.Add(orderDetail);
            }

            orders.Add(order);
        }

        return orders;
    }
}