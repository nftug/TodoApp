using Microsoft.AspNetCore.Components;
using Domain.Todos.Queries;
using Client.Shared.Extensions;
using Client.Components.Todos;

namespace Client.Pages;

public partial class TodoPage : ComponentBase
{
    [SupplyParameterFromQuery]
    [Parameter]
    public string? Page { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string? Q { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string? State { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string? Sort { get; set; } = null!;

    private TodoQueryParameter _parameter = new();
    private TodoTable? _table = null!;

    protected override void OnParametersSet()
    {
        _parameter = new TodoQueryParameter
        {
            Limit = 10,
            Page = Page.ParseAsPage(),
            Q = Q,
            State = State,
            Sort = Sort ?? new TodoQueryParameter().Sort
        };

        if (_table != null)
        {
            InvokeAsync(async () =>
            {
                await _table.OnParameterChanged();
            });
        }
    }
}
