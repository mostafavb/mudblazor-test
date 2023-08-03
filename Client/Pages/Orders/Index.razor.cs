using Infrastructure.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Text.Json;
using Ui.WebAssembly.Mappers;
using Ui.WebAssembly.Models;
using Ui.WebAssembly.Services;
using Ui.WebAssembly.Utilities;

namespace Ui.WebAssembly.Pages.Orders;

public partial class Index
{
    [Inject] ApiClient Api { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    ObservableCollection<OrderDto>? Orders;
    ObservableCollection<OrderDto>? PersistOrders;

    bool _isCellEditMode = true;
    List<string> _events = new();
    List<OrderTypeDto> _orderTypes = new();
    string _searchString;

    private PersistingComponentStateSubscription persistingSubscription;

    protected override async Task OnInitializedAsync()
    {
        var ordreTypeResponse = await Api.TypesAsync();
        if (ordreTypeResponse?.StatusCode == 200)
        {
            await InvokeAsync(() =>
            {
                var orderTypes = ordreTypeResponse.Result;
                _orderTypes = orderTypes.ToListOfDtos();
            });
        }
        ApiResponse<IList<Order>>? apiResponse = null;

        persistingSubscription = ApplicationState.RegisterOnPersisting(PersistForecasts);

        if (!ApplicationState.TryTakeFromJson<ObservableCollection<OrderDto>>("ordersdata", out var restoredOrders))
        {
            apiResponse ??= await Api.OrdersAllAsync();
            if (apiResponse?.StatusCode == 200)
            {
                await InvokeAsync(() =>
                {
                    Orders = new ObservableCollection<OrderDto>((apiResponse.Result).ToListDto());                    
                });
            }
        }
        else
        {
            Orders = restoredOrders!;
        }

        if (!ApplicationState.TryTakeFromJson<ObservableCollection<OrderDto>>("persistordersdata", out var restoredPersistOrders))
        {
            apiResponse ??= await Api.OrdersAllAsync();
            if (apiResponse?.StatusCode == 200)
            {
                await InvokeAsync(() =>
                {                    
                    PersistOrders = new ObservableCollection<OrderDto>(apiResponse.Result.ToListDto());
                });
            }
        }
        else
        {
            PersistOrders = restoredPersistOrders!;
        }

    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("activeFocus");
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

    private Task PersistForecasts()
    {
        ApplicationState.PersistAsJson("ordersdata", Orders);
        ApplicationState.PersistAsJson("persistordersdata", Orders);

        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        persistingSubscription.Dispose();
    }

    void StartedEditingItem(OrderDto item)
    {
        //_events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
    }

    void CanceledEditingItem(OrderDto item)
    {
        //_events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
    }

    void CommittedItemChanges(OrderDto item)
    {
        //_events.Insert(0, $"Event = CommittedItemChanges, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        //Console.WriteLine($"{System.Text.Json.JsonSerializer.Serialize(item)}");
    }
}
