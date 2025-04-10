namespace CustomerOrder.Common.DDD;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public int CompanyId { get; protected set; }

    public bool IsActive { get; set; }

    protected AggregateRoot() : base() { }

    protected AggregateRoot(TId id) : base(id) { }

    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void SetCompanyId(int companyId)
    {
        //if (CompanyId != 0)
        //    throw new ArgumentException("CompanyId cannot be modified once a value is assigned");

        if (companyId <= 0)
            throw new ArgumentException("CompanyId must have a positive number");

        CompanyId = companyId;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}