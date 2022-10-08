using Client.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Pages;

public partial class ImageTestPage : ComponentBase
{
    private string? _imageBase64Source;

    private bool _isLoading;
    private bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (value == _isLoading) return;
            _isLoading = value;
            StateHasChanged();
        }
    }

    private const int MaxFileSize = 10 * 1024 * 1024;

    private async Task OnSelectedFile(InputFileChangeEventArgs e)
    {
        if (!e.File.ContentType.StartsWith("image")) return;

        _imageBase64Source = null;

        using var stream = e.File.OpenReadStream(MaxFileSize);

        IsLoading = true;

        var source = await stream.ConvertToBase64StringAsync();
        _imageBase64Source = string.Format("data:image/png;base64,{0}", source);

        IsLoading = false;
    }
}