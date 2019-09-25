namespace ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ValueObject
    {
        #region Public Methods and Operators

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                throw new ArgumentException($"Invalid comparison of Value Objects of different types: {this.GetType()} and {obj.GetType()}");
            }

            var valueObject = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return this.GetEqualityComponents().Aggregate(
                1,
                (current, obj) =>
                    {
                        unchecked
                        {
                            return current * 23 + (obj?.GetHashCode() ?? 0);
                        }
                    });
        }

        #endregion

        #region Methods

        protected abstract IEnumerable<object> GetEqualityComponents();

        #endregion
    }
}