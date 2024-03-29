// Copyright (c) 2021 MudBlazor
// Reference: https://github.com/MudBlazor/MudBlazor
// Modified by V-nyang

// Released under the MIT License.
// See the LICENSE file in the project root for more information.

using Blazored.LocalStorage;

namespace Client.Services.UserPreferences;

public class UserPreferencesService
{
    private readonly ILocalStorageService _localStorage;
    private const string Key = "userPreferences";

    public UserPreferencesService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<UserPreferences> LoadUserPreferences()
        => await _localStorage.GetItemAsync<UserPreferences>(Key);

    public async Task SaveUserPreferences(UserPreferences userPreferences)
        => await _localStorage.SetItemAsync(Key, userPreferences);
}