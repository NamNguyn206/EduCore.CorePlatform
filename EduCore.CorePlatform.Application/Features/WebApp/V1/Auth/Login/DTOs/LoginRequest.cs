using EduCore.CorePlatform.Domain.Common.Constants.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Login.DTOs;

public class LoginRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public Guid? TenantId { get; set; } = null; // optional (nếu muốn chọn tenant khi login)
}
