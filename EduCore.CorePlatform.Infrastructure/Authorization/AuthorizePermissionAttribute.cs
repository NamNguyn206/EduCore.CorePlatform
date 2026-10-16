using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EduCore.CorePlatform.Infrastructure.Authorization;

public sealed class AuthorizePermissionAttribute : AuthorizeAttribute
{
    public const string POLICY_PREFIX = "PERMISSION:";

    public AuthorizePermissionAttribute(string permission)
    {
        Policy = POLICY_PREFIX + permission;
    }
}