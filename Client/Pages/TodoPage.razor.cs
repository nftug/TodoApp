using Microsoft.AspNetCore.Components;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Client.Services.Api;
using Client.Shared.Models;
using Client.Shared.Extensions;

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

    private Pagination<TodoResultDTO>? _todoItems;
    private bool _isLoading = false;

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
            if (showIndicator) _isLoading = true;

            _todoItems = await TodoApiService.GetList(param, showValidationError: true);

            _isLoading = false;
            StateHasChanged();
        });
    }

    private void OnEditItem()
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
