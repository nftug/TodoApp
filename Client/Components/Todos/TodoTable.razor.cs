using Client.Services.Api;
using Client.Services.Authentication;
using Domain.Todos.DTOs;
using Domain.Todos.Entities;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components.Todos;

public partial class TodoTable : DataTableBase<Todo, TodoResultDTO, TodoCommand, TodoQueryParameter>
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private AuthStoreService AuthStoreService { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

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

        await ApiService.Delete(item.Id);
        Snackbar.Add("Todoを削除しました。", Severity.Success);

        await _table.ReloadServerData();
    }

    private async Task ChangeState(TodoResultDTO item, TodoState state)
    {
        var newItem = await ((TodoApiService)ApiService).ChangeState(item.Id, state);
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
