using Microsoft.AspNetCore.Components;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Client.Models;
using Client.Services.Api;
using Client.Extensions;

namespace Client.Pages;

public partial class TodoPage : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;
    [Inject]
    private TodoApiService TodoApiService { get; set; } = null!;

    [SupplyParameterFromQuery]
    [Parameter]
    public string? Page { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string? Q { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string? State { get; set; } = null!;

    private Pagination<TodoResultDTO>? TodoItems { get; set; } = null;
    private bool IsLoading { get; set; }

    protected override void OnParametersSet()
    {
        FetchData();
    }

    private void FetchData(bool showIndicator = true)
    {
        var param = new TodoQueryParameter
        {
            Limit = 10,
            Page = Page.ParseAsPage(),
            Q = Q,
            State = State
        };

        InvokeAsync(async () =>
        {
            if (showIndicator) IsLoading = true;

            TodoItems = await TodoApiService.GetList(param);

            IsLoading = false;
            StateHasChanged();
        });
    }

    private void OnEditItem(TodoResultDTO item)
    {
        bool isParameterChanged =
            Page.ParseAsPage() != 1
             || !string.IsNullOrEmpty(Q)
             || !string.IsNullOrEmpty(State);

        if (isParameterChanged)
            Navigation.NavigateTo(Navigation.Uri.Split('?')[0]);
        else
            FetchData(showIndicator: false);
    }
}
