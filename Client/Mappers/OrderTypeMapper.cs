using Infrastructure.Shared.Models;
using Ui.WebAssembly.Models;
using System.Linq;

namespace Ui.WebAssembly.Mappers;

internal static class OrderTypeMapper
{
    public static OrderTypeDto ToDto(this OrderType ot) =>
        new OrderTypeDto { Id = ot.Id, Name = ot.Name };

    public static List<OrderTypeDto> ToListOfDtos(this IList<OrderType> ots) =>
       ots.Select(p => new OrderTypeDto { Id = p.Id, Name = p.Name, }).ToList();


    public static List<KeyValuePair<int, string>> ToListOfPairs(this IList<OrderType> list) =>
       list.Select(s => new KeyValuePair<int, string>(s.Id, s.Name)).ToList();
        
       

}
