using System.Text.RegularExpressions;

namespace Infrastructure.Shared.QuerySearch.Models;

public class SearchField<T>
{
    public string? Param { get; set; }
    public List<QuerySearchExpression<T>> Node { get; set; }
    public CombineMode CombineMode { get; set; }

    public SearchField(string? param) : this()
    {
        Param = param;
        CombineMode = GetCombineMode(param);
    }

    public SearchField()
    {
        Node = new List<QuerySearchExpression<T>>();
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
