using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Identity;

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<string>? RoleNames { get; set; }
    public List<Guid>? TenantIds { get; set; }
}