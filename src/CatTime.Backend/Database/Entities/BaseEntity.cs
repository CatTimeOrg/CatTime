namespace CatTime.Backend.Database.Entities;

public abstract class BaseEntity : IEquatable<BaseEntity?>
{
    public int Id { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseEntity);
    }

    public bool Equals(BaseEntity? other)
    {
        return other is not null &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        return EqualityComparer<BaseEntity>.Default.Equals(left, right);
    }

    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !(left == right);
    }
}

