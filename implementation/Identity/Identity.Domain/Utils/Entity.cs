namespace Identity.Domain.Utils
{
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string PublicId { get; protected set; }
        public virtual DateTimeOffset CreatedAt { get; protected set; }


        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            if (string.IsNullOrEmpty(PublicId) || string.IsNullOrEmpty(other.PublicId))
                return false;

            return PublicId == other.PublicId;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (this.GetType().ToString() + PublicId).GetHashCode();
        }
    }
}