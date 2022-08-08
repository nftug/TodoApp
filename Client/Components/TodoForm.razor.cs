using Application.Todos.Models;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Components;

public partial class TodoForm : MyComponentBase
{
    [Parameter]
    public TodoCommandDTO Command { get; set; } = null!;
    [Parameter]
    public EventCallback<TodoCommandDTO> CommandChanged { get; set; }
    [Parameter]
    public EventCallback<EditContext> OnValidSubmit { get; set; }
    [Parameter]
    public EventCallback OnResetForm { get; set; }
    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private EditContext EditContext = null!;
    private TodoCommandDTO _command = null!;

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Command);
        _command = Command;
    }

    protected override void OnParametersSet()
    {
        if (_command != Command)
        {
            _command = Command;

            EditContext = new EditContext(Command);
            EditContext.MarkAsUnmodified();
        }
    }

    public void ResetForm()
    {
        Command = new TodoCommandDTO();
        CommandChanged.InvokeAsync(Command);
    }

    private async Task OnDeleteHandler()
    {
        await OnDelete.InvokeAsync((Guid)Command.Id!);
        ResetForm();
    }

    private async Task OnResetFormHandler()
    {
        ResetForm();
        await OnResetForm.InvokeAsync();
    }
}
