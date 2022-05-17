namespace Domain.Shared;

public abstract class ValueObject<TSelf, TValue> where TSelf : ValueObject<TSelf, TValue>
{
    public TValue Value { get; }

    protected ValueObject(TValue value)
    {
        Value = value;
    }

    protected abstract bool EqualsCore(TSelf other);

    public override bool Equals(object? obj)
    {
        var vo = obj as TSelf;
        if (vo == null) return false;

        return EqualsCore(vo);
    }

    public static bool operator ==(ValueObject<TSelf, TValue>? vo1, ValueObject<TSelf, TValue>? vo2)
    {
        return Equals(vo1, vo2);
    }

    public static bool operator !=(ValueObject<TSelf, TValue>? vo1, ValueObject<TSelf, TValue>? vo2)
    {
        return !Equals(vo1, vo2);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
