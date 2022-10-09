namespace Client.Shared.Models;

public class VirtualizedItem<T>
{
    public T Result { get; init; } = default!;
    public int Index { get; init; }
}
