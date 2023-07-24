using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Ui.WebAssembly.Mappers;
using Ui.WebAssembly.Models;
using Ui.WebAssembly.Services;
using Ui.WebAssembly.Utils;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Ui.WebAssembly.Pages.Orders;

public partial class Index
{
    [Inject] ApiClient Api { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    ObservableCollection<OrderDto>? Orders;
    ObservableCollection<OrderDto>? PersistOrders;

    bool _isCellEditMode = true;
    bool _isDataLoaded = false;
    List<string> _events = new();
    string _searchString;

    protected override async Task OnInitializedAsync()
    {
        var response = await Api.OrdersAllAsync();
        if (response?.StatusCode == 200)
        {
            await InvokeAsync(() =>
            {
                Orders = new ObservableCollection<OrderDto>((response.Result).ToListDto());
                PersistOrders = new ObservableCollection<OrderDto>(response.Result.ToListDto());
                _isDataLoaded = true;
            });


        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isDataLoaded)
        {
            await JSRuntime.InvokeVoidAsync("activeFocus");
            _isDataLoaded = false;
        }
    }
    async Task CheckDataDifference()
    {
        var comparer = DynamicEqualityComparerFactory.Create<OrderDto>(
            nameof(OrderDto.OrderDate),
            nameof(OrderDto.CustomerAddress),
            nameof(OrderDto.CustomerName)
            );


        var diffs = (await Task.Run(() => Orders.Except(PersistOrders, comparer))).ToList();
        if (diffs.Count > 0)
        {
            string changedNote = $"There are {diffs.Count} rows changed.";
            Console.WriteLine(changedNote);

            foreach (var item in diffs)
            {
                Console.WriteLine($" => Before: {JsonSerializer.Serialize(PersistOrders.FirstOrDefault(f => f.Id == item.Id))}");
                Console.WriteLine($" => After: {JsonSerializer.Serialize(item)}");
                Console.WriteLine($"     ");
            }
        }
    }

    Func<OrderDto, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.CustomerAddress.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.CustomerName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ($"{x.TotalPrice} {x.Id} {x.OrderDate}".Contains(_searchString))
            return true;

        return false;
    };

    async Task HandleKeyDown(KeyboardEventArgs e)
    {
        string action = e.Key switch
        {
            "ArrowUp" => "up",
            "ArrowDown" => "down",
            "ArrowLeft" => "left",
            "ArrowRight" => "right",
            _ => string.Empty
        };

        if (!string.IsNullOrWhiteSpace(action))
            await JSRuntime.InvokeVoidAsync("mudDataGridInterop.setFocusOnCell", action);
    }

    void StartedEditingItem(OrderDto item)
    {
        _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
    }

    void CanceledEditingItem(OrderDto item)
    {
        _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
    }

    void CommittedItemChanges(OrderDto item)
    {
        //_events.Insert(0, $"Event = CommittedItemChanges, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        Console.WriteLine($"{System.Text.Json.JsonSerializer.Serialize(item)}");
    }
}
