using ApiClient.Repositories;
using MudBlazorTemplates1.Shared.Models;

namespace ApiClient.Endpoints;

public static class Orders
{
    public static IEndpointRouteBuilder RegisterOrderEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IOrdersRepository repository) =>
        {
            var orders = repository.GetAllOrders();

            return await Task.FromResult(TypedResults.Ok(orders.OrderBy(o=>o.Id).ThenBy(t=>t.OrderDate)));
        })
            .Produces<List<Order>>();

        group.MapGet("/{id}", async (int id, IOrdersRepository repository) =>
        {
            var order = repository.GetOrderById(id);
            if (order != null)
                return await Task.FromResult(Results.Ok(order));
            else
                return await Task.FromResult(Results.NotFound());
        })
            .Produces<Order>()
            .Produces(400)
            .Produces(404);

        group.MapPost("/", async (Order order, IOrdersRepository repository) =>
        {
            repository.AddOrder(order);
            return await Task.FromResult(Results.Created($"/api/orders/{order.Id}", order));
        })
            .Produces<Order>()
            .Produces(400);

        group.MapPut("/{id}", async (int id, Order order, IOrdersRepository repository) =>
        {
            var existingOrder = repository.GetOrderById(id);
            if (existingOrder != null)
            {
                existingOrder.CustomerName = order.CustomerName;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.OrderTypeId = order.OrderTypeId;
                existingOrder.OrderType = order.OrderType;
                existingOrder.OrderDetails = order.OrderDetails;

                return await Task.FromResult(TypedResults.Ok(existingOrder));
            }
            else
            {
                return await Task.FromResult(Results.NotFound());
            }
        })  .Produces<Order>()
            .Produces(400)
            .Produces(404);

        group.MapDelete("/{id}", async (int id, IOrdersRepository repository) =>
        {
            var order = repository.GetOrderById(id);
            if (order != null)
            {
                repository.DeleteOrder(order);
                return await Task.FromResult(Results.Ok());
            }
            else
            {
                return await Task.FromResult(Results.NotFound());
            }
        })
            .Produces(400)
            .Produces(404);

        group.MapGet("/types", async (IOrdersRepository repository) =>
        {
            var types = repository.GetOrderTypes();

            return await Task.FromResult(TypedResults.Ok(types.OrderBy(o => o.Id)));
        })
            .Produces<List<OrderType>>();

        return group;
    }
}
