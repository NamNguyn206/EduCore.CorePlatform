using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs;

public class UserProfileDto
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public DateTime? CreatedAt { get; set; }
    public UserType? UserType { get; set; }
    public LanguageDto? Language { get; set; }
    public UserVerificationStatus? VerificationStatus { get; set; }
}