// Copyright (c) 2021 MudBlazor
// Reference: https://github.com/MudBlazor/MudBlazor
// Modified by V-nyang

// Released under the MIT License.
// See the LICENSE file in the project root for more information.

using Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Shared.Components;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    private MudThemeProvider _mudThemeProvider = null!;

    protected override void OnInitialized()
    {
        LayoutService.MajorUpdateOccurred += LayoutServiceOnMajorUpdateOccurred;
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await ApplyUserPreferences();
            StateHasChanged();
        }
    }

    private async Task ApplyUserPreferences()
    {
        var defaultDarkMode = await _mudThemeProvider.GetSystemPreference();
        await LayoutService.ApplyUserPreferences(defaultDarkMode);
    }

    public void Dispose()
    {
        LayoutService.MajorUpdateOccurred -= LayoutServiceOnMajorUpdateOccurred;
        GC.SuppressFinalize(this);
    }

    private void LayoutServiceOnMajorUpdateOccurred(object? sender, EventArgs e) => StateHasChanged();
}