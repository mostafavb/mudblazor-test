namespace ApiClient.Endpoints;

public static class RegisterAllEndpoint
{
    public static IEndpointRouteBuilder RegisterEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        endpoints.MapGroup("/api/weathers")
            .WithOpenApi()
            .RegisterWeatherEndpoints();

        endpoints.MapGroup("/api/orders")
            .WithOpenApi()
            .RegisterOrderEndpoints();
        
        endpoints.MapGroup("/api/products")
            .WithOpenApi()
            .RegisterProductsEndpoints();

        return endpoints;
    }
}
