using Bogus;
using MudBlazorTemplates1.Shared.Models;

namespace MudBlazorTemplates1.Server.Services;

public class OrdersRepository
{
    private readonly List<Order> _orders;

    public OrdersRepository()
    {
        FakeDataGenerator fakeDataGenerator = new();

        _orders = fakeDataGenerator.GenerateFakeOrders(20);
    }

    public List<Order> GetAllOrders()
    {
        return _orders;
    }

    public Order GetOrderById(int id)
    {
        return _orders.FirstOrDefault(o => o.Id == id) ?? new();
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
}

public class FakeDataGenerator
{
    public List<Order> GenerateFakeOrders(int count)
    {
        var faker = new Faker();

        var orderTypes = new List<OrderType>()
        {
            new OrderType(1, "Online"),
            new OrderType(2, "In store"),
            new OrderType(3, "Wharehouse")
        };

        var products = new List<Product>()
        {
            new Product(1, "Product 1", 10.0m),
            new Product(2, "Product 2", 15.0m),
            new Product(3, "Product 3", 20.0m),
            new Product(4, "Product 4", 25.0m)
        };

        var orders = new List<Order>();

        for (int i = 0; i < count; i++)
        {
            var order = new Order()
            {
                Id = faker.Random.Int(1, 100),
                CustomerName = faker.Name.FullName(),
                OrderDate = faker.Date.PastDateOnly(),
                OrderTypeId = faker.PickRandom(orderTypes).Id,
                OrderDetails = new List<OrderDetail>()
            };

            var numOrderDetails = faker.Random.Int(1, 5);

            for (int j = 0; j < numOrderDetails; j++)
            {
                var product = faker.PickRandom(products);

                var orderDetail = new OrderDetail()
                {
                    Id = faker.Random.Int(1, 100),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Amount = faker.Random.Decimal(1.0m, 10.0m),
                    Product = product
                };

                order.OrderDetails.Add(orderDetail);
            }

            orders.Add(order);
        }

        return orders;
    }
}