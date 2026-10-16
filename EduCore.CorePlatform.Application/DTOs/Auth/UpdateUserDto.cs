using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Identity;

public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserStatus? Status { get; set; }
}