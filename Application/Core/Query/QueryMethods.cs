namespace Application.Core.Query;

public class QueryMethods
{
    public static void ForEachKeyword(string? param, Action<string> func)
    {
        if (string.IsNullOrEmpty(param)) return;

        string paramLower = param.ToLower().Replace("　", " ");
        string[] paramArray = paramLower.Split(' ');

        foreach (var _param in paramArray)
        {
            if (!string.IsNullOrEmpty(_param))
            {
                func(_param);
            }
        }
    }
}
