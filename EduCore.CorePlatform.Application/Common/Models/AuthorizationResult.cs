using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public sealed class AuthorizationResult
{
    public bool IsAllowed { get; }
    public string Message { get; }

    private AuthorizationResult(bool isAllowed, string message)
    {
        IsAllowed = isAllowed;
        Message = message;
    }

    public static AuthorizationResult Allow(string message)
        => new(true, message);

    public static AuthorizationResult Deny(string message)
        => new(false, message);
}
