using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Exceptions;

public sealed class BusinessException : Exception
{
    public string ErrorCode { get; }

    public BusinessException(string errorCode)
        => ErrorCode = errorCode;
}
