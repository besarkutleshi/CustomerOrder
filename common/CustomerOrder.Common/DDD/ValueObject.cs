namespace CustomerOrder.Common.DDD;
public abstract class ValueObject
{
    /// <summary>
    /// Returns the atomic values of the value object.
    /// Derived classes must implement this method to provide a sequence
    /// of all values that participate in equality comparisons.
    /// </summary>
    /// <returns>A sequence of objects representing the value object's components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        // Start with a seed and combine each component's hash code
        return GetEqualityComponents().Aggregate(17, (current, component) =>
        {
            unchecked
            {
                return current * 23 + (component?.GetHashCode() ?? 0);
            }
        });
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }
}