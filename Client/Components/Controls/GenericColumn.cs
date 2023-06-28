using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorTemplates1.WebAssembly.Utilities;
using System.Reflection;

namespace MudBlazorTemplates1.WebAssembly.Components.Controls;


public partial class GenericColumn<T> : Column<T>
{
    [Parameter] public string Field { get; set; }
    [Parameter] public string? Format { get; set; }

    protected override void OnParametersSet()
    {
        CellTemplate = GetCellTemplate();
        EditTemplate = GetEditTemplate();
    }

    private RenderFragment<CellContext<T>> GetCellTemplate()
    {
        RenderFragment<CellContext<T>> ret_val = new RenderFragment<CellContext<T>>(GetCellTemplate_Content);
        return ret_val;
    }

    private RenderFragment GetCellTemplate_Content(CellContext<T> context) => builder =>
    {
        Type myType = typeof(T);
        PropertyInfo propertyInfo = myType.GetProperty(Field);
        var actualType = propertyInfo.PropertyType;
        var actualValue = propertyInfo.GetValue(context.Item);

        string tmp_str = string.Empty;

        if (actualValue != null)
        {
            if (TypeIdentifier.IsIntegralNumberype(actualType) )
                tmp_str = ((Int32)actualValue).ToString("N0");

            else if (actualType == typeof(decimal))
                tmp_str = ((decimal)actualValue).ToString("N2");

            else if (TypeIdentifier.IsDateTime(actualType))
                tmp_str = ((DateTime)actualValue).ToShortDateString();
            else
                tmp_str = actualValue.ToString();
        }
        builder.OpenElement(0, "Value");
        builder.AddContent(1, tmp_str);
        builder.CloseElement();
    };

    private RenderFragment<CellContext<T>> GetEditTemplate()
    {
        RenderFragment<CellContext<T>> ret_val = new RenderFragment<CellContext<T>>(GetEditTemplate_Content);
        return ret_val;
    }

    private RenderFragment GetEditTemplate_Content(CellContext<T> context)
    {
        return builder =>
        {
            if (Hidden)
            {
                builder.OpenElement(0, "Value");
                builder.AddContent(1, "");
                builder.CloseElement();
            }
            else
            {

                Type myType = typeof(T);
                PropertyInfo propertyInfo = myType.GetProperty(Field);
                var actualType = propertyInfo.PropertyType;
                var actualValue = propertyInfo.GetValue(context.Item);

                if (actualType == typeof(decimal) || actualType == typeof(decimal?))
                {
                    builder.OpenComponent<MudTextField<decimal?>>(0);
                    builder.AddAttribute(0, "ValueChanged", EventCallback.Factory.Create<decimal?>(this, HandleFieldChange));
                }
                else if (actualType == typeof(Int32) || actualType == typeof(Int32?))
                {
                    builder.OpenComponent<MudTextField<Int32?>>(0);
                    builder.AddAttribute(0, "ValueChanged", EventCallback.Factory.Create<int?>(this, HandleFieldChange));
                }
                else if (actualType == typeof(DateTime) || actualType == typeof(DateTime?))
                {
                    builder.OpenComponent<MudTextField<DateTime?>>(0);
                    builder.AddAttribute(0, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, HandleFieldChange));
                }
                else if (actualType == typeof(bool) || actualType == typeof(bool?))
                {
                    builder.OpenComponent<MudCheckBox<bool?>>(0);
                    builder.AddAttribute(0, "CheckedChanged", EventCallback.Factory.Create<bool?>(this, HandleFieldChange));
                }
                else
                {
                    builder.OpenComponent<MudTextField<string?>>(0);
                    builder.AddAttribute(0, "ValueChanged", EventCallback.Factory.Create<string?>(this, HandleFieldChange));
                }

                builder.AddAttribute(1, "Label", Title);
                builder.AddAttribute(2, "Value", actualValue);
                builder.AddAttribute(3, "Disabled", !IsEditable);
                //builder.AddAttribute(4, "visibility", Hidden ? "hidden" : "visible");

                builder.CloseComponent();

                Value = context.Item;
            }
        };
    }

    private void HandleFieldChange(bool? obj)
    {
        HandleFieldChange(obj.ToString() ?? null);
    }

    private void HandleFieldChange(decimal? obj)
    {
        HandleFieldChange(obj.ToString() ?? null);
    }

    private void HandleFieldChange(int? obj)
    {
        HandleFieldChange(obj.ToString() ?? null);
    }

    private void HandleFieldChange(DateTime? obj)
    {
        HandleFieldChange(obj.ToString() ?? null);
    }

    private void HandleFieldChange(string? obj)
    {
        Type myType = typeof(T);
        PropertyInfo propertyInfo = myType.GetProperty(Field);
        var actualType = propertyInfo.PropertyType;
        propertyInfo.SetValue(Value, obj);
    }

    protected override object PropertyFunc(T item)
    {
        throw new NotImplementedException();
    }

    protected override void SetProperty(object item, object value)
    {
        throw new NotImplementedException();
    }

    protected override Type PropertyType => base.PropertyType;

    protected override string ContentFormat => Format;


    protected override object? CellContent(T item) => item;


}
