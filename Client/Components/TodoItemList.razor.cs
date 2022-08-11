using Application.Todos.Models;
using Client.Extensions;
using Client.Services.Api;
using Client.Services.Authentication;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components;

public partial class TodoItemList : ComponentBase
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
    public IEnumerable<TodoResultDTO> Items { get; set; } = null!;
    [Parameter]
    public EventCallback OnEditItem { get; set; }
    [Parameter]
    public EventCallback OnDeleteItem { get; set; }
    [Parameter]
    public EventCallback OnChangeState { get; set; }
    [Parameter]
    public bool IsLoading { get; set; }

    private MudMessageBox? DeleteConfirm { get; set; }
    private string SearchText { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        SearchText = Navigation.QueryString("q") ?? string.Empty;
    }

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

    private async Task EditItem(TodoResultDTO item)
    {
        if (!IsOwnedByUser(item)) return;

        var parameters = new DialogParameters { ["Command"] = new TodoCommand(item) };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small };
        var dialog = DialogService.Show<TodoEditDialog>("Todoの編集", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled) await OnEditItem.InvokeAsync();
    }

    private async Task DeleteItem(TodoResultDTO item)
    {
        if (DeleteConfirm == null) return;
        if (await DeleteConfirm.Show() == null) return;

        await TodoApiService.Delete(item.Id);
        Snackbar.Add("Todoを削除しました。", Severity.Success);

        await OnDeleteItem.InvokeAsync();
    }

    private async Task ChangeState(TodoResultDTO item, TodoState state)
    {
        var newItem = await TodoApiService.ChangeState(item.Id, state);
        Snackbar.Add("Todoの状態を変更しました。", Severity.Success);

        await OnChangeState.InvokeAsync();
    }

    private void DoSearch()
    {
        var querySuffix = !string.IsNullOrWhiteSpace(SearchText) ? $"?q={SearchText}" : null;
        var uri = $"{Navigation.Uri.Split('?')[0]}{querySuffix}";
        Navigation.NavigateTo(uri);
    }
}
