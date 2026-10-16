using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public List<string>? Permissions { get; set; }
}
