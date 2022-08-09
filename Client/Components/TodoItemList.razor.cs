using Application.Todos.Models;
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

    [Parameter]
    public IEnumerable<TodoResultDTO> Items { get; set; } = null!;
    [Parameter]
    public EventCallback<TodoResultDTO> OnEditItem { get; set; }
    [Parameter]
    public EventCallback OnDeleteItem { get; set; }
    [Parameter]
    public EventCallback<TodoResultDTO> OnChangeState { get; set; }

    private MudMessageBox? DeleteConfirm { get; set; }

    private static Color GetTodoChipColor(TodoResultDTO item)
        => item.State == TodoState.Doing
            ? Color.Tertiary
            : item.State == TodoState.Done
            ? Color.Success
            : Color.Primary;

    private static string GetTodoIcon(TodoResultDTO item)
        => item.State == TodoState.Doing
            ? Icons.Outlined.IndeterminateCheckBox
            : item.State == TodoState.Done
            ? Icons.Outlined.CheckBox
            : Icons.Outlined.CheckBoxOutlineBlank;

    private bool IsDisabledChangeState(TodoResultDTO item, TodoState state)
        => !IsOwnedByUser(item) || item.State.Value == state.Value;

    private bool IsOwnedByUser(TodoResultDTO item) => item.OwnerUserId == AuthStoreService.UserId;

    private async Task EditItem(TodoResultDTO item)
    {
        if (!IsOwnedByUser(item)) return;

        var parameters = new DialogParameters { ["Command"] = new TodoCommandDTO(item) };
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
        if (newItem == null) return;
        Snackbar.Add("Todoの状態を変更しました。", Severity.Success);

        await OnChangeState.InvokeAsync(item);
    }
}
