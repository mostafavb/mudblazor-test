using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using Ui.WebAssembly.Extensions;
using Ui.WebAssembly.Mappers;
using Ui.WebAssembly.Models;
using Ui.WebAssembly.Services;
using static Ui.WebAssembly.Enumes.GenericDataGrid;

namespace Ui.WebAssembly.Pages.Orders;

public partial class IndexByComponent
{
    [Inject] ApiClient Api { get; set; }
    // fields

    ObservableCollection<OrderDto> Items;
    ObservableCollection<OrderDto> PersistOrders;
    Dictionary<string, object> externalDatas;
    Dictionary<ActionTypes, IList<OrderDto>> changes;

    // events

    protected override async Task OnInitializedAsync()
    {
        var ordreTypeResponse = await Api.TypesAsync();
        if (ordreTypeResponse?.StatusCode == 200)
        {
            await InvokeAsync(() =>
            {
                var orderTypes = ordreTypeResponse.Result;
                externalDatas ??= new();
                externalDatas[nameof(OrderDto.OrderTypeId)] = orderTypes.ToListOfPairs();
            });
        }

        var orderResponse = await Api.OrdersAllAsync();
        if (orderResponse?.StatusCode == 200)
        {
            await Task.Run(() => Items = new ObservableCollection<OrderDto>((orderResponse.Result).ToListDto()));
            await Task.Run(() => PersistOrders = new ObservableCollection<OrderDto>(orderResponse.Result.ToListDto()));
        }

    }

    async Task<IList<OrderDto>> GetDifferences()
    {
        changes = new();

        //var originOfChangedRows = await GetAllChangesAsync();
        var deletedRows = PersistOrders.FindDifferencesByKeies(Items).ToList();
        var addedRows = Items.FindDifferencesByKeies(PersistOrders).ToList();
        var editesdRows = (await GetEdittedRowsAsync()).Except(addedRows).ToList();

        changes.Add(ActionTypes.Deleted, deletedRows);
        changes.Add(ActionTypes.Added, addedRows);
        changes.Add(ActionTypes.Edited, editesdRows);


        //var editedRows = await GetEdittedRowsAsync();

        return await Task.FromResult(deletedRows.Concat(addedRows).Concat(editesdRows).ToList());
    }

    async Task ApplyChanges()
    {
        await Task.CompletedTask;
    }
    // methods

    async Task DataChangedByFile(IList<OrderDto> orders)
    {
        await InvokeAsync(() =>
        {
            Items = new ObservableCollection<OrderDto>(orders.ToList());
        });
        //PersistOrders = new ObservableCollection<OrderDto>(orderDtos.ToList());
        StateHasChanged();
    }

    async ValueTask<IList<OrderDto>> GetAllChangesAsync() =>
            await Task.FromResult(PersistOrders?.FindDifferences(Items ?? new(), true) ?? Enumerable.Empty<OrderDto>().ToList());


    async Task<IList<OrderDto>> GetEdittedRowsAsync() =>
           await Task.FromResult(Items?.FindDifferences(PersistOrders ?? new()) ?? Enumerable.Empty<OrderDto>().ToList());
}
