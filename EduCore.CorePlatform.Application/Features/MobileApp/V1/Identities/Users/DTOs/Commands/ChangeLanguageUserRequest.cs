using EduCore.CorePlatform.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Commands;

public class ChangeLanguageUserRequest : IdRequest
{
    public string LanguageCode { get; init; } = default!;
}
