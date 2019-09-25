namespace ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     Source http://grabbagoft.blogspot.fr/2007/06/generic-value-object-equality.html
    /// </summary>
    /// <typeparam name="T">Immutable class</typeparam>
    public abstract class SimpleValueObject<T> : IEquatable<T>
        where T : SimpleValueObject<T>
    {
        public static bool operator ==(SimpleValueObject<T> x, SimpleValueObject<T> y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(SimpleValueObject<T> x, SimpleValueObject<T> y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as T;

            return this.Equals(other);
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            var t = this.GetType();
            var otherType = other.GetType();

            if (t != otherType)
            {
                return false;
            }

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                    {
                        return false;
                    }
                }
                else if (!value1.Equals(value2))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var fields = this.GetFields();

            var startValue = 17;
            var multiplier = 59;

            var hashCode = startValue;

            foreach (var field in fields)
            {
                var value = field.GetValue(this);

                if (value != null)
                {
                    hashCode = hashCode * multiplier + value.GetHashCode();
                }
            }

            return hashCode;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = this.GetType();

            var fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                t = t.BaseType;
            }

            return fields;
        }
    }
}