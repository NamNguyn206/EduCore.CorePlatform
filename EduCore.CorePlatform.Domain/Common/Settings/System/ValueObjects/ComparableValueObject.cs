using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Settings.System.ValueObjects;

public abstract class ComparableValueObject<T> : SimpleValueObject<T>, IComparable<ComparableValueObject<T>>
    where T : IComparable<T>
{
    protected ComparableValueObject(T value) : base(value)
    {
    }

    public int CompareTo(ComparableValueObject<T>? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    public static bool operator <(ComparableValueObject<T>? left, ComparableValueObject<T>? right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator >(ComparableValueObject<T>? left, ComparableValueObject<T>? right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator <=(ComparableValueObject<T>? left, ComparableValueObject<T>? right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >=(ComparableValueObject<T>? left, ComparableValueObject<T>? right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}