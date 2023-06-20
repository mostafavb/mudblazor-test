using ApiClient.Repositories;
using MudBlazorTemplates1.Shared.Models;

namespace ApiClient.Endpoints;

public static class Products
{
    public static IEndpointRouteBuilder RegisterProductsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IOrdersRepository repository) =>
        {
            var products = repository.GetProducts();

            return await Task.FromResult(TypedResults.Ok(products.OrderBy(o => o.Id)));
        })
          .Produces<List<Product>>();

        return group;
    }
}
