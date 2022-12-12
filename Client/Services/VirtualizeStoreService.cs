using Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Services;

public class VirtualizeStoreService<T>
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigation;

    public VirtualizeStoreService(IJSRuntime jsRuntime, NavigationManager navigation)
    {
        _jsRuntime = jsRuntime;
        _navigation = navigation;
    }

    public event Action? OnChangeCacheList;

    private List<VirtualizedItem<T>> _cacheList = new();
    public List<VirtualizedItem<T>> CacheList
    {
        get => _cacheList;
        set
        {
            _cacheList = value;
            OnChangeCacheList?.Invoke();
        }
    }

    public double ScrollY { get; private set; }

    public async Task SetScrollYAndNavigateAsync(string uri)
    {
        ScrollY = await _jsRuntime.InvokeAsync<double>("getScrollY");
        _navigation.NavigateTo(uri);
    }

    public async Task ResumeCurrentAsync()
    {
        await Task.Delay(500);
        await _jsRuntime.InvokeVoidAsync("setScrollY", ScrollY);
        // ScrollY = 0;
    }
}
