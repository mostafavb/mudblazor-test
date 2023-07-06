using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace MudBlazorTemplates1.WebAssembly.Components.Bases;

[CascadingTypeParameter(nameof(T))]
public partial class SCDataGrid<T> : MudDataGrid<T>
{
    public int MosyCounter { get; set; }

    public List<Column<T>> RenderedColumns => base.RenderedColumns;
}
