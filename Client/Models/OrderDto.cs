using MiniExcelLibs.Attributes;
using Ui.WebAssembly.Utilities;

namespace Ui.WebAssembly.Models;

public class OrderDto : IModel
{
    [DataGridModel(IsEditable = false, IsReadOnly = true, IsSortable = true, IsFilterable = false, Title = "Id")]
    public int Id { get; set; }

    [ExcelFormat("yyyy/MM/dd")]
    [DataGridModel(IsEditable = true, IsFilterable = true, IsSortable = true, Title = "Order Date", Format = "yyyy/MM/dd")]
    public DateTime? OrderDate { get; set; }

    [ExcelColumn(Ignore = true)]
    [ExcelFormat("yyyy/MM/dd")]
    [DataGridModel(IsEditable = true, IsFilterable = true, IsSortable = true, Title = "Payment Date", Format = "yyyy/MM/dd")]
    public DateTime? PaymentDate { get; set; }

    [DataGridModel(IsEditable = true, IsSortable = true, IsFilterable = true, Title = "Customer Name")]
    public string? CustomerName { get; set; }

    [DataGridModel(IsHiden = true)]
    public string? CustomerAddress { get; set; }

    [DataGridModel(IsHiden = false, IsEditable = true, IsFilterable = true, DataType = typeof(List<KeyValuePair<int, string>>))]
    public int OrderTypeId { get; set; }

    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Total Price")]
    public decimal TotalPrice { get; set; }


    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Zip Code")]
    public string? ZipCode { get; set; }

    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Visitor Name")]
    public string? VisitorName { get; set; }

    [DataGridModel(IsHiden = true, IsFilterable = true, IsReadOnly = true, IsEditable = false, Title = "Factor Id")]
    public int FactorId { get; set; }

    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    public decimal Remains { get; set; }

    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    public int PaymentId { get; set; }


    [DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    public string? BankAccountingId { get; set; }




    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Country")]
    //public string? Country { get; set; }

    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "City")]
    //public string? City { get; set; }


    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    //public decimal BasePrice { get; set; }

    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    //public decimal Payment { get; set; }


    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    //public decimal LastPayment { get; set; }


    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    //public bool IsClosed { get; set; }

    //[DataGridModel(IsSortable = true, IsFilterable = true, IsEditable = true)]
    //public bool HasAccounting { get; set; }


    [DataGridModel(IsHiden = false, IsEditable = true, Title = "Ordered")]
    public bool IsOrder { get; set; } = true;

    public Task Validation()
    {
        throw new NotImplementedException();
    }
}
