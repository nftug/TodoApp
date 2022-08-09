using Microsoft.AspNetCore.Components;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Client.Models;
using Client.Services.Api;

namespace Client.Pages;

public partial class TodoPage : MyComponentBase
{
    [Inject]
    private TodoApiService TodoApiService { get; set; } = null!;

    [SupplyParameterFromQuery]
    [Parameter]
    public string Page { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string Q { get; set; } = null!;
    [SupplyParameterFromQuery]
    [Parameter]
    public string State { get; set; } = null!;

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
            Page = ParsePage(Page),
            Q = Q,
            State = ParseState(State)
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
            ParsePage(Page) != 1
             || !string.IsNullOrEmpty(Q)
             || !string.IsNullOrEmpty(State);

        if (isParameterChanged)
            Navigation.NavigateTo(Navigation.Uri.Split('?')[0]);
        else
            FetchData(showIndicator: false);
    }

    // TODO: あとで文字列でも検索できるようにする (QueryParameterから書き換える)
    private static int? ParseState(string? value)
        => ParseIntParam(value, (x, canParse) => canParse ? x : null);
}
