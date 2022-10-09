using Blazored.LocalStorage;
using Client.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Pages;

public partial class ImageTestPage : ComponentBase
{
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = null!;

    private string? _imageBase64Source;

    public const string StorageKey = "imageBase64Source";

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        IsLoading = true;
        await Task.Delay(500);

        _imageBase64Source = await LocalStorageService.GetItemAsync<string>(StorageKey);
        IsLoading = false;
    }

    private async Task OnSelectedPicture(InputFileChangeEventArgs e)
    {
        if (!e.File.ContentType.StartsWith("image")) return;

        _imageBase64Source = null;

        using var stream = e.File.OpenReadStream(MaxFileSize);

        IsLoading = true;

        var source = await stream.ConvertToBase64StringAsync();
        _imageBase64Source = string.Format("data:{0};base64,{1}", e.File.ContentType, source);

        await LocalStorageService.SetItemAsync(StorageKey, _imageBase64Source);

        IsLoading = false;
    }

    private async Task OnDeletedPicture()
    {
        _imageBase64Source = null;
        await LocalStorageService.RemoveItemAsync(StorageKey);
    }
}