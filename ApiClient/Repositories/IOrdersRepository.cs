using MudBlazorTemplates1.Shared.Models;

namespace ApiClient.Repositories;
public interface IOrdersRepository
{
    void AddOrder(Order order);
    void DeleteOrder(Order order);
    List<Order> GetAllOrders();
    Order GetOrderById(int id);
    List<Product> GetProducts();
    List<OrderType> GetOrderTypes();
}