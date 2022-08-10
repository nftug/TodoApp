namespace Client.Extensions;

public static class ParamExtension
{
    public static int? ParseAsIntParam(this string? value, Func<int, bool, int?> func)
    {
        bool canParse = int.TryParse(value, out int _value);
        return func(_value, canParse);
    }

    public static int ParseAsPage(this string? value)
        => (int)ParseAsIntParam(value, (x, _) => x > 0 ? x : 1)!;
}
