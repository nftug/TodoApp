using Application.Todos.Models;
using Client.Services.Api;
using Domain.Todos.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Client.Components.Todos;

public partial class TodoEditDialog : ComponentBase
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public TodoCommand Command { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IApiService<TodoResultDTO, TodoCommand, TodoQueryParameter> TodoApiService { get; set; } = null!;

    private EditContext _editContext = null!;
    private bool _isSaving = false;

    private bool IsNewData => Command.Id == null;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Command);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SubmitTodo()
    {
        if (IsNewData)
        {
            _isSaving = true;
            var result = await TodoApiService.Create(Command);
            _isSaving = false;
            if (result != null) Snackbar.Add("Todoを作成しました。", Severity.Success);
        }
        else
        {
            _isSaving = true;
            var result = await TodoApiService.Put((Guid)Command.Id!, Command);
            _isSaving = false;
            if (result != null) Snackbar.Add("Todoを編集しました。", Severity.Success);
        }

        MudDialog.Close(DialogResult.Ok(Command));
    }

    private bool ValidateEditForm
        => IsNewData
           ? _editContext.IsModified() && _editContext.Validate()
           : _editContext.Validate();
}
