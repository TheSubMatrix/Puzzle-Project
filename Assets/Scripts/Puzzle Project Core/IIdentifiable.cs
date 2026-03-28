using System;
public interface IIdentifiable : IEquatable<IIdentifiable>
{
    bool IEquatable<IIdentifiable>.Equals(IIdentifiable other)
    {
        return other != null && other.ID == ID;
    }

    Guid ID { get; }
}
