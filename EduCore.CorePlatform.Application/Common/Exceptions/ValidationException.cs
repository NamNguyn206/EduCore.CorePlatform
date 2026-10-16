using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Exceptions;

public class ValidationException : ApplicationException
{
    public List<ValidationError> Errors { get; }

    public ValidationException()
        : base("Validation failed")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(IEnumerable<ValidationError> failures)
        : this()
    {
        Errors = failures
            .Select(f => new ValidationError
            {
                PropertyName = f.PropertyName,
                ErrorMessage = f.ErrorMessage,
                AttemptedValue = f.AttemptedValue,
                ErrorCode = f.ErrorCode
            })
            .ToList();
    }

    public ValidationException(List<ValidationError> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}

public class ValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public object? AttemptedValue { get; set; }
    public string? ErrorCode { get; set; }
}