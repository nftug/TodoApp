using System.Collections.Specialized;
using System.Web;

namespace Client.Services;

public class PageInfoService
{
    public event Action? QueriesChanged;
    private NameValueCollection _queries = new();
    public NameValueCollection Queries
    {
        get => _queries;
        private set
        {
            if (value == _queries) return;
            _queries = value;
            QueriesChanged?.Invoke();
        }
    }

    public void SetQueries(string path)
    {
        var pathSplitted = path.Split('?');
        Queries = pathSplitted.Length > 1
            ? HttpUtility.ParseQueryString(pathSplitted[1])
            : new();
    }

    public event Action? RouteChanged;
    public string? CurrentUri { get; private set; }
    public string? PreviousUri { get; private set; }
    public string? CurrentRoute => CurrentUri?.Split('?').First();
    public string? PreviousRoute => PreviousUri?.Split('?').First();

    public void SetCurrentUri(string currentUri)
    {
        PreviousUri = CurrentUri;
        CurrentUri = currentUri;

        if (CurrentRoute == PreviousRoute)
            RouteChanged?.Invoke();
    }

    public event Action? TitleChanged;
    private string? _title;
    public string? Title
    {
        get => _title;
        private set
        {
            if (value == _title) return;
            _title = value;
            TitleChanged?.Invoke();
        }
    }

    public void SetTitle(string? title)
    {
        Title = title;
    }

    public event Action? BackButtonChanged;
    private bool _hasBackButton;
    public bool HasBackButton
    {
        get => _hasBackButton;
        private set
        {
            if (value == _hasBackButton) return;
            _hasBackButton = value;
            BackButtonChanged?.Invoke();
        }
    }

    public void SetHasBackButton(bool value)
    {
        HasBackButton = value;
    }
}
