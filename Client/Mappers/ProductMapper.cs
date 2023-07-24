using Infrastructure.Shared.Models;
using Ui.WebAssembly.Models;

namespace Ui.WebAssembly.Mappers;

internal static class ProductMapper
{
    public static ProductDto ToDto(this Product product) =>
        new ProductDto { Id = product.Id, Name = product.Name };

    public static List<ProductDto> ToListDto(this IList<Product> products) =>
       products.Select(p => new ProductDto { Id = p.Id, Name = p.Name, }).ToList();
}
