using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Identity;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
}