using MiniExcelLibs.Attributes;
using Ui.WebAssembly.Utilities;

namespace Ui.WebAssembly.Models;

public class OrderDto
{
    [GenericDataGridPrefrences(IsKey = true, IsEditable = false, IsReadOnly = true, IsSortable = true, IsFilterable = false, Title = "Id")]
    public int Id { get; set; }

    [ExcelFormat("yyyy/MM/dd")]
    [GenericDataGridPrefrences(IsEditable = true, IsFilterable = true, IsSortable = true, Title = "Order Date", Format = "yyyy/MM/dd")]
    public DateTime? OrderDate { get; set; }


    [GenericDataGridPrefrences(IsEditable = true, IsFilterable = true, IsSortable = true, Title = "Payment Date", Format = "yyyy/MM/dd")]
    [ExcelColumn(Ignore = true)]
    public DateTime? PaymentDate { get; set; }

    [GenericDataGridPrefrences(IsEditable = true, IsSortable = true, IsFilterable = true, Title = "Customer Name")]
    //[ExcelColumn(Ignore = true)]
    public string? CustomerName { get; set; }

    [GenericDataGridPrefrences(IsHiden = true)]
    public string? CustomerAddress { get; set; }

    [GenericDataGridPrefrences(IsHiden = false, IsEditable = true, IsFilterable = true, DataType = typeof(List<KeyValuePair<int, string>>))]
    public int OrderTypeId { get; set; }

    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Total Price")]
    public decimal TotalPrice { get; set; }


    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Zip Code")]
    public string? ZipCode { get; set; }

    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true, Title = "Visitor Name")]
    public string? VisitorName { get; set; }

    [GenericDataGridPrefrences(IsHiden = true, IsFilterable = true, IsReadOnly = true, IsEditable = false, Title = "Factor Id")]
    public int FactorId { get; set; }

    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true)]
    public decimal Remains { get; set; }

    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true)]
    public int PaymentId { get; set; }


    [GenericDataGridPrefrences(IsSortable = true, IsFilterable = true, IsEditable = true)]
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


    [GenericDataGridPrefrences(IsHiden = false, IsEditable = true, Title = "Ordered")]
    public bool IsOrder { get; set; } = true;


}
