// Reference: https://stackoverflow.com/a/61688018

using System.Net;
using System.Text.Json;
using Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Components.Common;

public class ServerValidator<TErrorDetails> : ComponentBase
{
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(ServerValidator<ErrorDetails>)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerValidator<TErrorDetails>)} " +
                $"inside an EditForm.");
        }
    }

    public async void Validate(HttpResponseMessage response, object model)
    {
        var messages = new ValidationMessageStore(CurrentEditContext);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var body = await response.Content.ReadAsStringAsync();
            var validationProblemDetails = JsonSerializer.Deserialize<ErrorDetails>(body);

            if (validationProblemDetails?.Errors != null)
            {
                messages.Clear();

                foreach (var error in validationProblemDetails.Errors)
                {
                    var fieldIdentifier = new FieldIdentifier(model, error.Key);
                    messages.Add(fieldIdentifier, error.Value);
                }
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }
}