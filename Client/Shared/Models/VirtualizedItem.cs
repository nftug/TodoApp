namespace Client.Shared.Models;

public class VirtualizedItem<T>
{
    public T Result { get; init; } = default!;
    public int Index { get; init; }
}

public static class VirtualizedItemExtensions
{
    public static List<VirtualizedItem<T>> GetUnionList<T>(
        this ICollection<VirtualizedItem<T>> virtualizedItems,
        IEnumerable<T> items,
        int startIndex
    )
    {
        var _results = items
                .Select((x, i) => new VirtualizedItem<T>
                {
                    Result = x,
                    Index = i + startIndex
                })
                .OrderBy(x => x.Index);

        return virtualizedItems
            .Union(_results).DistinctBy(x => x.Index).OrderBy(x => x.Index).ToList();
    }

    public static IEnumerable<T> GetItems<T>(
        this ICollection<VirtualizedItem<T>> virtualizedItems,
        int startIndex,
        int count
    )
        => virtualizedItems.Skip(startIndex).Take(count).Select(x => x.Result);
}
