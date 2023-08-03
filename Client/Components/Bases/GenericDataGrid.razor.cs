using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MiniExcelLibs;
using MudBlazor;
using System.Reflection;
using Ui.WebAssembly.Extensions;
using Ui.WebAssembly.Utilities;

namespace Ui.WebAssembly.Components.Bases;

[CascadingTypeParameter(nameof(T))]
public partial class GenericDataGrid<T>
{

    #region Dependncy Injection
    [Inject] IJSRuntime JS { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    #endregion

    #region Parameters

    [Parameter]
    [EditorRequired]
    public IList<T> Items { get; set; }

    [Parameter] public bool ReadOnly { get; set; } = false;

    [Parameter] public DataGridEditMode DataGridEditMode { get; set; } = DataGridEditMode.Cell;

    [Parameter] public bool HasSearchbar { get; set; } = false;

    [Parameter] public bool Filterable { get; set; } = false;

    [Parameter] public string HeaderTitle { get; set; } = string.Empty;

    [Parameter] public Func<Task<IList<T>?>>? GetDifferences { get; set; }

    [Parameter] public EventCallback<IList<T>> OnDataChanged { get; set; }

    [Parameter] public EventCallback OnApplyChangedData { get; set; }

    [Parameter] public bool ShowDeleteRowButton { get; set; } = false;

    [Parameter] public bool ShowEditRowButton { get; set; } = false;

    [Parameter] public bool FixedHeader { get; set; } = false;

    [Parameter] public bool ShowMenue { get; set; } = true;

    [Parameter] public bool ShowCheckDifference { get; set; } = false;

    //[Parameter] public bool ShowHiddenItemsInDownloadFile { get; set; } = false;

    [Parameter] public ExcelType DownloadFileType { get; set; } = ExcelType.XLSX;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? ExternalData { get; set; } = null;
    #endregion


    // Properties


    #region Fields

    string? _searchString;
    bool isFormOpened;
    int bageCounter;
    bool showFileUploadDialog = false;
    bool showProgress = true;
    bool hasVirtualize = false;
    bool columnsShouldRender = true;

    DialogOptions _dialogOptions = new DialogOptions { CloseButton = false, DisableBackdropClick = true };

    IBrowserFile? _file;

    MudDataGrid<T> grid;
    List<PropertyInfo> _propertyInfos;
    #endregion


    #region Events

    protected override async Task OnParametersSetAsync()
    {
        if (Items is not null && Items.Count > 0)
        {
            _propertyInfos = (PropertyInfoHelpers.GetPropertyInfos(Items.First())
                                .Where(w => !w.GetCustomAttribute<GenericDataGridPrefrencesAttribute>()!.IsHiden) ?? Enumerable.Empty<PropertyInfo>())
                                .ToList();
            hasVirtualize = Items.Count > 70;
            //LogTypeAttributes();

        }
        await Task.CompletedTask;
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("activeFocus");
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            showProgress = false;
            columnsShouldRender = false;
            //StateHasChanged();
        }
    }


    /// <summary>
    /// triggers the OnApplyChangedData event
    /// </summary>
    /// <returns></returns>
    async Task ApplyChangedData()
    {
        await OnApplyChangedData.InvokeAsync();
    }


    /// <summary>
    /// upload the file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    async Task OnFilesChanged(InputFileChangeEventArgs file)
    {

        await InvokeAsync(() => _file = file.File);
    }

    /// <summary>
    /// when click commits on download in the menue this method triggers
    /// </summary>
    /// <returns></returns>
    async Task OnDownloadClicked()
    {
        await InvokeAsync(() => DownloadFile());
    }

    /// <summary>
    /// this method calls when delete event commits
    /// </summary>
    /// <param name="item"></param>
    async Task OnDeleteRowClick(T item)
    {
        await InvokeAsync(() =>
        {
            Items.Remove(item);
            bageCounter++;
        });
    }

    /// <summary>
    /// this method calls when edit button is clicked
    /// </summary>
    /// <param name="item"></param>
    async Task OnEditRowClick(T item)
    {
        isFormOpened = true;
        await grid.SetEditingItemAsync(item);
    }


    /// <summary>
    /// when add new item calls then this method triggers
    /// </summary>
    /// <returns></returns>
    async void OnClickAddItem()
    {
        await AddNewItem();
    }

    /// <summary>
    /// commit the add event, it adds a new item of type T
    /// </summary>
    /// <param name="item"></param>
    async Task OnCommitedNewItem(T item)
    {
        await InvokeAsync(() =>
        {
            Items.Add(item);
            isFormOpened = false;
            //columnsShouldRender = false;
            bageCounter++;
        });

    }

    /// <summary>
    /// commit the canceled event on new item dilog
    /// </summary>
    /// <param name="item"></param>
    async void OnCanceledNewItem(T item)
    {
        //columnsShouldRender = false;
        isFormOpened = false;
        //await CheckDataDifferenceHandler();

    }


    /// <summary>
    /// this method calls the deleget method 'GetDifferences()' and check changes in the grid data source
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    async Task CheckDataDifferenceHandler(MouseEventArgs e = null)
    {
        await InvokeAsync(async () =>
        {
            //showProgress = true;
            var diffs = await GetDifferences() ?? Enumerable.Empty<T>();
            bageCounter = diffs?.Count() ?? 0;
            //showProgress = false;
        });

    }


    /// <summary>
    /// commit inline changes for every inputs that has IsEditable attribute
    /// </summary>
    /// <param name="item"></param>
    async Task OnCellChangesCommitted(T item)
    {
        await InvokeAsync(() =>
        {
            bageCounter++;
        });
    }

    /// <summary>
    /// submit uploaded file
    /// </summary>
    async Task OnSubmitFileClick()
    {
        await InvokeAsync(async () =>
        {
            if (_file is not null)
            {
                showProgress = true;
                if (await FileUpload())
                {
                    //if (!hasVirtualize)
                    //    await CheckDataDifferenceHandler();
                    //else
                    //    bageCounte++;
                }
                showFileUploadDialog = false;
                showProgress = false;
            }
            else
            {
                Snackbar.Add($"No file selected.", Severity.Info);
            }
        });
    }

    #endregion


    #region Methods

    /// <summary>
    /// creat a file from the grid data source, file's extension sets as input parameter. default extension is .xlsx
    /// </summary>
    /// <returns></returns>
    async Task DownloadFile()
    {
        showProgress = true;

        #region old code
        /*
        object items = ShowHiddenItemsInDownloadFile ? Items :
            Items.Select(item => new
            {
                QuailifiedProperties = item.GetType()
                                    .GetProperties()
                                    .Where(p =>
                                    {
                                        var excelAttrs = p.GetCustomAttribute<ExcelColumnAttribute>();
                                        if (excelAttrs?.Ignore ?? false)
                                            return false;
                                        var dataGridAttrs = p.GetCustomAttribute<DataGridModelAttribute>();
                                        return (dataGridAttrs is not null && (!dataGridAttrs.IsHiden));
                                    })
                                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(item))


                //var instance = Activator.CreateInstance<T>();
                //foreach (var property in properties)
                //    property.SetValue(instance, property.GetValue(s), null);
                //return instance;

            }).Select(s => s.QuailifiedProperties).ToList();

        }
        */
        #endregion

        using var memoryStream = new MemoryStream();

        await memoryStream.SaveAsAsync(Items, excelType: DownloadFileType);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamRef = new DotNetStreamReference(stream: memoryStream);

        var fileName = (string.IsNullOrWhiteSpace(HeaderTitle) ?
                                        nameof(T).ToLower().Replace("dto", "") :
                                        HeaderTitle
                                ) + $".{DownloadFileType.ToString().ToLower()}";

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        showProgress = false;
    }

    /// <summary>
    /// open a dialog and generate inputs based on the type of T and its attributes.
    /// </summary>
    /// <returns></returns>
    async Task AddNewItem()
    {
        await InvokeAsync(async () =>
        {
            var item = (T)Activator.CreateInstance(typeof(T))!;

            if (item is not null)
            {
                isFormOpened = true;
                //columnsShouldRender = true;
                await grid.SetEditingItemAsync(item);
                //StateHasChanged();
            }
        });
    }

    /// <summary>
    /// make a deleget method for those properties have IsFilterable attribute.
    /// </summary>
    Func<T, bool> _quickFilter => x =>
    {
        if (_propertyInfos is null)
            return false;

        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        return _propertyInfos?.Where(p =>
        {
            var attributes = p.GetCustomAttribute<GenericDataGridPrefrencesAttribute>();
            return attributes is not null && (attributes.IsFilterable && !attributes.IsHiden);
        })
        .Any(property => ($"{property.GetValue(x)}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))) ?? false;
    };


    /// <summary>
    /// opens the File upload dialog
    /// </summary>
    void ShowFileUploadDialog()
    {
        _file = null;
        showFileUploadDialog = true;
    }


    /// <summary>
    /// upload the file and set grid data source by new data. if process fails then nothing will happen.
    /// acceptable files extension: .csv, .xlsx
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    async Task<bool> FileUpload()
    {
        try
        {

            long maxFileSizeKB = 1024 * 1024 * 100;  // file size is in MB
            if (_file?.Size > maxFileSizeKB)
            {
                Snackbar.Add($"File size exceeds the maximum allowed limit.<br><strong>The maximum file size is {maxFileSizeKB / 1024 * 1024} MB</strong>", Severity.Info);
                return false;
            }

            using var stream = _file?.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var extension = Path.GetExtension(_file?.Name);
            if (extension.IsEmpty())
            {
                Snackbar.Add($"Warning: <strong>Invalid file type</strong><br>Chosen file is not valid, Please select a valid file.", Severity.Warning);
                return false;
            }
            var uploadedItems = await ms.QueryAsync<T>(excelType: extension.ToLower().Contains("csv") ? ExcelType.CSV : ExcelType.XLSX);

            if (uploadedItems is not null && uploadedItems.Count() > 0)
            {
                await OnDataChanged.InvokeAsync(uploadedItems.ToList());
                await CheckDataDifferenceHandler();
                //bageCounte = uploadedItems.Count();
                //Items = uploadItems.ToList();
            }
            else
            {
                Snackbar.Add($"Invalid file type. Please select a valid file.", Severity.Info);
                return false;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: <strong>{ex.Message}</strong><br>The uploading process failed, check your file content and try again.", Severity.Warning);
            return false;
        }
        return true;
    }


    /// <summary>
    /// this method invokes from javascript and will define dialog state.
    /// </summary>
    /// <param name="returned"></param>
    /// <returns></returns>
    [Obsolete("no need to this method, it implements by css", false)]
    [JSInvokable]
    public Task HandleAutoGeneratedDiv(bool returned)
    {
        isFormOpened = returned;
        StateHasChanged();
        return Task.CompletedTask;
    }

    #endregion
}
