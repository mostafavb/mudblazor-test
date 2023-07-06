using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudBlazorTemplates1.WebAssembly.Components.Bases;


[CascadingTypeParameter("T")]
public partial class MvbDataGrid<T> : MudDataGrid<T>
{
    List<Column<T>> _columns => base.RenderedColumns;

}
