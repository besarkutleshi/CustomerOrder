namespace CustomerOrder.Common.DDD;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }

    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }

    public string? CreatedByUserId { get; set; } = null!;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    public string? UpdateByUserId { get; set; } = null!;
    public DateTimeOffset? UpdateDate { get; set; }

    public void SetCreatedByUserId(string userId)
    {
        CreatedByUserId = userId;
        CreatedDate = DateTimeOffset.Now;
    }

    public void SetUpdatedByUserId(string userId)
    {
        UpdateByUserId = userId;
        UpdateDate = DateTimeOffset.Now;
    }

    public string DateCreated() => CreatedDate.ToString();
    public string DateUpdated() => UpdateDate.ToString() ?? "";

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