namespace Infrastructure.Shared.QuerySearch.Models;

public class Keyword
{
    public string Value { get; init; } = string.Empty;
    public CombineMode CombineMode { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();

    private Keyword() { }

    public Keyword
        (string value, CombineMode combineMode, Guid id)
    {
        Value = value;
        CombineMode = combineMode;
        Id = id;
    }

    public static Keyword CreateDummy()
        => new() { CombineMode = CombineMode.And };

    public static IEnumerable<Keyword> CreateFromRawString(string? param)
    {
        if (string.IsNullOrWhiteSpace(param))
            yield break;

        string[] paramArray = param.Replace("\u3000", " ").Split(' ');

        CombineMode mode = CombineMode.And;
        Guid blockId = Guid.NewGuid();

        for (int i = 0; i < paramArray.Length; i++)
        {
            CombineMode oldMode = mode;

            if (string.IsNullOrWhiteSpace(paramArray[i]) || paramArray[i] == "OR")
                continue;

            // 次の項目を読んで、現在がOR区分に当てはまるか判定
            if (i + 1 < paramArray.Length)
                mode = paramArray[i + 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // 前の項目を読んで、現在がOR区分に当てはまるか判定
            if (i - 1 > 0)
                mode = paramArray[i - 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // OR/ANDが変更されたら、ブロックを変える
            if (oldMode != mode)
                blockId = Guid.NewGuid();

            yield return new(paramArray[i].ToLower(), mode, blockId);
        }
    }
}
