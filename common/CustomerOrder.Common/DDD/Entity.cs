namespace CustomerOrder.Common.DDD;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }

    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (Entity<TId>)obj;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!);
    }
}