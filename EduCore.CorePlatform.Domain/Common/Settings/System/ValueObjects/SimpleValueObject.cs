using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Settings.System.ValueObjects;

public abstract class SimpleValueObject<T> : ValueObject
{
    public T Value { get; }

    protected SimpleValueObject(T value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}