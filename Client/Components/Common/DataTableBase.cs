using System.Text.RegularExpressions;
using Client.Services.Api;
using Domain.Shared.Entities;
using Domain.Shared.Models;
using Domain.Shared.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components.Todos;

public abstract class DataTableBase<TDomain, TResultDTO, TCommandDTO, TQueryParameter> : ComponentBase
    where TDomain : ModelBase
    where TQueryParameter : IQueryParameter<TDomain>, new()
{
    [Inject]
    protected IApiService<TResultDTO, TCommandDTO, TQueryParameter> ApiService { get; set; } = null!;
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    [Parameter]
    public TQueryParameter Parameter { get; set; } = new();
    [Parameter]
    public EventCallback OnLoaded { get; set; }

    public Pagination<TResultDTO>? Data = null!;

    protected MudTable<TResultDTO> _table = null!;

    protected virtual SortDirection GetSortDirection(string value)
    {
        var regex = new Regex("^-");
        string sortKey = regex.Replace(Parameter.Sort, "");
        if (value != sortKey) return SortDirection.None;

        bool isDescending = regex.IsMatch(Parameter.Sort);
        return isDescending ? SortDirection.Descending : SortDirection.Ascending;
    }

    public virtual async Task OnParameterChanged()
    {
        // ページネーションの更新に必要
        await Task.Delay(5);

        // ソート状態を初期化する (ヘッダーのソート表示の更新に必要)
        _table.Context.InitializeSorting();
        await _table.ReloadServerData();
    }

    protected virtual async Task<TableData<TResultDTO>> ServerReload(TableState state)
    {
        string stateSort = state.SortDirection == SortDirection.Descending
            ? $"-{state.SortLabel}" : state.SortLabel;

        if (stateSort != Parameter.Sort)
        {
            string? sortKey;
            if (state.SortDirection != SortDirection.None)
            {
                string sortPrefix = state.SortDirection == SortDirection.Descending ? "-" : string.Empty;
                Parameter.Sort = sortPrefix + state.SortLabel;
                sortKey = Parameter.Sort;
            }
            else
            {
                Parameter.Sort = new TQueryParameter().Sort;
                sortKey = null;
            }

            Navigation.NavigateTo(Navigation.GetUriWithQueryParameters(
                new Dictionary<string, object?> { { "sort", sortKey }, { "page", null } }
            ));
        }

        StateHasChanged();  // ローディング状態を更新する
        Data = await ApiService.GetList(Parameter, showValidationError: true);

        var totalItems = (int)Data!.TotalItems;
        var items = Data.Results;

        await OnLoaded.InvokeAsync();

        return new TableData<TResultDTO> { TotalItems = totalItems, Items = items };
    }
}
