using System.Text.RegularExpressions;
using Application.Todos.Models;
using Client.Services.Api;
using Client.Services.Authentication;
using Client.Shared.Models;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components.Todos;

public partial class TodoTable : ComponentBase
{
    [Inject]
    private TodoApiService TodoApiService { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private AuthStoreService AuthStoreService { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Parameter]
    public TodoQueryParameter Parameter { get; set; } = new TodoQueryParameter();
    [Parameter]
    public EventCallback OnLoaded { get; set; }

    public Pagination<TodoResultDTO>? Data = null!;

    private MudTable<TodoResultDTO> _table = null!;
    private MudMessageBox _deleteConfirm = null!;

    private static Color GetTodoChipColor(TodoResultDTO item)
    {
        var state = new TodoState(item.State);
        return state == TodoState.Doing
            ? Color.Tertiary
            : state == TodoState.Done
            ? Color.Success
            : Color.Primary;
    }

    private static string GetTodoIcon(TodoResultDTO item)
    {
        var state = new TodoState(item.State);
        return state == TodoState.Doing
            ? Icons.Outlined.IndeterminateCheckBox
            : state == TodoState.Done
            ? Icons.Outlined.CheckBox
            : Icons.Outlined.CheckBoxOutlineBlank;
    }

    private bool IsDisabledChangeState(TodoResultDTO item, TodoState state)
        => !IsOwnedByUser(item) || new TodoState(item.State) == state;

    private bool IsOwnedByUser(TodoResultDTO item) => item.OwnerUserId == AuthStoreService.UserId;

    private SortDirection GetSortDirection(string value)
    {
        var regex = new Regex("^-");
        string sortKey = regex.Replace(Parameter.Sort, "");
        if (value != sortKey) return SortDirection.None;

        bool isDescending = regex.IsMatch(Parameter.Sort);
        return isDescending ? SortDirection.Descending : SortDirection.Ascending;
    }

    public async Task OnParameterChanged()
    {
        // ページネーションの更新に必要
        await Task.Delay(5);

        // ソート状態を初期化する (ヘッダーのソート表示の更新に必要)
        _table.Context.InitializeSorting();
        await _table.ReloadServerData();
    }

    public async Task RefreshTable()
    {
        bool isParameterChanged =
            Parameter.Page != 1
             || !string.IsNullOrEmpty(Parameter.Q)
             || !string.IsNullOrEmpty(Parameter.State)
             || Parameter.Sort != new TodoQueryParameter().Sort;

        if (isParameterChanged)
            Navigation.NavigateTo(Navigation.Uri.Split('?')[0]);
        else
            await _table.ReloadServerData();
    }

    private async Task<TableData<TodoResultDTO>> ServerReload(TableState state)
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
                Parameter.Sort = new TodoQueryParameter().Sort;
                sortKey = null;
            }

            Navigation.NavigateTo(Navigation.GetUriWithQueryParameters(
                new Dictionary<string, object?> { { "sort", sortKey }, { "page", null } }
            ));
        }

        StateHasChanged();  // ローディング状態を更新する
        Data = await TodoApiService.GetList(Parameter, showValidationError: true);

        var totalItems = (int)Data!.TotalItems;
        var items = Data.Results;

        await OnLoaded.InvokeAsync();

        return new TableData<TodoResultDTO> { TotalItems = totalItems, Items = items };
    }

    private async Task EditItem(TodoResultDTO item)
    {
        if (!IsOwnedByUser(item)) return;

        var parameters = new DialogParameters { ["Command"] = new TodoCommand(item) };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small };
        var dialog = DialogService.Show<TodoEditDialog>("Todoの編集", parameters, options);
        var result = await dialog.Result;

        if (result.Cancelled) return;
        await RefreshTable();
    }

    private async Task DeleteItem(TodoResultDTO item)
    {
        if (await _deleteConfirm.Show() == null) return;

        await TodoApiService.Delete(item.Id);
        Snackbar.Add("Todoを削除しました。", Severity.Success);

        await _table.ReloadServerData();
    }

    private async Task ChangeState(TodoResultDTO item, TodoState state)
    {
        var newItem = await TodoApiService.ChangeState(item.Id, state);
        Snackbar.Add("Todoの状態を変更しました。", Severity.Success);

        await _table.ReloadServerData();
    }

    private void DoSearch()
    {
        var querySuffix = !string.IsNullOrWhiteSpace(Parameter.Q) ? $"?q={Parameter.Q}" : null;
        var uri = $"{Navigation.Uri.Split('?')[0]}{querySuffix}";
        Navigation.NavigateTo(uri);
    }
}
