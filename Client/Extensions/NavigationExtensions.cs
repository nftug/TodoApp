using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace Client.Extensions;

public static class NavigationExtensions
{
    // get entire querystring name/value collection
    public static NameValueCollection QueryString(this NavigationManager navigationManager)
    {
        return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
    }

    // get single querystring value with specified key
    public static string? QueryString(this NavigationManager navigationManager, string key)
    {
        return navigationManager?.QueryString()[key];
    }
}
