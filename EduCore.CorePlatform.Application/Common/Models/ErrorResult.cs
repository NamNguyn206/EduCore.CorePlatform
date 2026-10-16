using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public sealed class ErrorResult
{
    public string Code { get; }
    public string Message { get; }

    public ErrorResult(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
