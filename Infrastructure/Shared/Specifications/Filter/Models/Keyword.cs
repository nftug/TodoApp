using System.Text.RegularExpressions;

namespace Infrastructure.Shared.Specifications.Filter.Models;

internal class Keyword
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Value { get; init; } = string.Empty;
    public CombineMode CombineMode { get; init; }
    public bool InQuotes { get; init; } = false;

    public static IEnumerable<Keyword> CreateFromRawString(string? param)
    {
        if (string.IsNullOrWhiteSpace(param))
            yield break;

        // 二重引用符に囲まれた文字列を抽出
        var quotesRegex = new Regex("\"(.*?)\"");
        var quotesList = quotesRegex.Matches(param);
        for (int i = 0; i < quotesList.Count; i++)
            param = quotesRegex.Replace(param, $"#{i}#", 1);

        string[] paramWords = param.Replace("\u3000", " ").Split(' ');

        var combineMode = CombineMode.And;
        Guid blockId = Guid.NewGuid();

        for (int i = 0; i < paramWords.Length; i++)
        {
            string current = paramWords[i];
            if (string.IsNullOrWhiteSpace(current) || current == "OR")
                continue;

            var matchQuote = Regex.Match(current, "^#([0-9]+)#$");
            bool inQuotes = matchQuote.Success;
            if (inQuotes)
            {
                int index = int.Parse(matchQuote.Groups[1].Value);
                current = quotesList[index].Groups[1].Value;
            }

            var combineModeOld = combineMode;

            // 次の項目を読んで、現在がOR区分に当てはまるか判定
            if (i + 1 < paramWords.Length)
                combineMode = paramWords[i + 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // 前の項目を読んで、現在がOR区分に当てはまるか判定
            if (i - 1 > 0)
                combineMode = paramWords[i - 1] == "OR" ? CombineMode.OrElse : CombineMode.And;

            // OR/ANDが変更されたら、ブロックを変える
            if (combineModeOld != combineMode)
                blockId = Guid.NewGuid();

            yield return new()
            {
                Id = blockId,
                Value = inQuotes ? current : current.ToLower(),
                CombineMode = combineMode,
                InQuotes = inQuotes
            };
        }
    }
}
