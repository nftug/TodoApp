using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Models;

public class MyComponentBase : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    protected AuthenticationStateProvider Authentication { get; set; } = null!;

    protected static int? ParseIntParam(string? value, Func<int, bool, int?> func)
    {
        bool canParse = int.TryParse(value, out int _value);
        return func(_value, canParse);
    }

    protected static int? ParsePage(string? value)
        => ParseIntParam(value, (x, _) => x > 0 ? x : 1);
}
