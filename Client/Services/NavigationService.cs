using System.Collections.Specialized;
using System.Web;

namespace Client.Services;

public class NavigationService
{
    public event Action? Notify;

    private NameValueCollection _queries = new();
    public NameValueCollection Queries
    {
        get => _queries;
        private set
        {
            if (value == _queries) return;
            _queries = value;
            Notify?.Invoke();
        }
    }

    public void SetQueries(string path)
    {
        var pathSplitted = path.Split('?');
        Queries = pathSplitted.Length > 1
            ? HttpUtility.ParseQueryString(pathSplitted[1])
            : new();
    }
}
