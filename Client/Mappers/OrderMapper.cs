using Infrastructure.Shared.Models;
using Ui.WebAssembly.Models;

namespace Ui.WebAssembly.Mappers;

public static class OrderMapper
{
    public static OrderDto ToDto(this Order model) =>
        new OrderDto
        {
            CustomerAddress = model.CustomerAddress,
            CustomerName = model.CustomerName,
            Id = model.Id,
            OrderDate = new DateTime(model.OrderDate.Year, model.OrderDate.Month, model.OrderDate.Day),
            OrderTypeId = model.OrderTypeId,
            TotalPrice = model.TotalPrice,

            ZipCode = model.ZipCode,
            VisitorName = model.VisitorName,
            Remains = model.Remains,
            PaymentId = model.PaymentId,
            PaymentDate = new DateTime(model.PaymentDate.Year, model.PaymentDate.Month, model.PaymentDate.Day),
            BankAccountingId = model.BankAccountingId,
            FactorId = model.FactorId,
            IsOrder = true,
            //BasePrice = model.BasePrice,
            //City = model.City,
            //Country = model.Country,
            //HasAccounting = model.HasAccounting,
            //IsClosed = model.IsClosed,
            //LastPayment = model.LastPayment,
            //Payment = model.Payment
        };

    public static List<OrderDto> ToListDto(this IList<Order> models) =>
      models.Select(model =>
        {
            var order = new OrderDto
            {
                CustomerAddress = model.CustomerAddress,
                CustomerName = model.CustomerName,
                Id = model.Id,
                OrderDate = new DateTime(model.OrderDate.Year, model.OrderDate.Month, model.OrderDate.Day),
                OrderTypeId = model.OrderTypeId,
                TotalPrice = model.TotalPrice,
                IsOrder = true,
                ZipCode = model.ZipCode,
                VisitorName = model.VisitorName,
                Remains = model.Remains,
                PaymentId = model.PaymentId,
                PaymentDate = new DateTime(model.PaymentDate.Year, model.PaymentDate.Month, model.PaymentDate.Day),
                BankAccountingId = model.BankAccountingId,
                //BasePrice = model.BasePrice,
                //City = model.City,
                //Country = model.Country,
                //FactorId = model.FactorId,
                //HasAccounting = model.HasAccounting,
                //IsClosed = model.IsClosed,
                //LastPayment= model.LastPayment,
                //Payment = model.Payment
            };

            return order;
        }).ToList();

}
