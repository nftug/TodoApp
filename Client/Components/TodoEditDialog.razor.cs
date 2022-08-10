using Application.Todos.Models;
using Client.Services.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Client.Components;

public partial class TodoEditDialog : ComponentBase
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public TodoCommandDTO Command { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private TodoApiService TodoApiService { get; set; } = null!;

    private EditContext EditContext = null!;

    private bool IsNewData => Command.Id == null;

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Command);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task SubmitTodo()
    {
        if (!EditContext.Validate()) return;

        if (IsNewData)
        {
            var result = await TodoApiService.Create(Command);
            if (result != null) Snackbar.Add("Todoを作成しました。", Severity.Success);
        }
        else
        {
            var result = await TodoApiService.Put((Guid)Command.Id!, Command);
            if (result != null) Snackbar.Add("Todoを編集しました。", Severity.Success);
        }

        MudDialog.Close(DialogResult.Ok(Command));
    }

    private bool ValidateEditForm
        => IsNewData ? (EditContext.IsModified() && EditContext.Validate()) : EditContext.Validate();
}
