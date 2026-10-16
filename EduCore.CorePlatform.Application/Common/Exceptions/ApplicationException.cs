using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Exceptions;

public class ApplicationException : Exception
{
    public string? ErrorCode { get; }
    public Dictionary<string, object>? AdditionalData { get; }

    public ApplicationException(string message)
        : base(message)
    {
    }

    public ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ApplicationException(string errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public ApplicationException(string errorCode, string message, Dictionary<string, object> additionalData)
        : base(message)
    {
        ErrorCode = errorCode;
        AdditionalData = additionalData;
    }
}