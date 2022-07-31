using System.Text.RegularExpressions;

namespace Infrastructure.Shared.Specifications.Filter.Models;

internal class SearchField<T>
{
    public string? Param { get; set; }
    public IEnumerable<QueryFilterExpression<T>> Node { get; set; }
    public CombineMode CombineMode { get; set; }

    public SearchField(string? param) : this()
    {
        Param = param;
        CombineMode = GetCombineMode(param);
    }

    public SearchField()
    {
        Node = new List<QueryFilterExpression<T>>();
    }

    private static CombineMode GetCombineMode(string? param)
    {
        if (string.IsNullOrWhiteSpace(param))
            return CombineMode.And;
        else if (Regex.IsMatch(param, "^OR( |\u3000)"))
            return CombineMode.OrElse;
        else
            return CombineMode.And;
    }
}
