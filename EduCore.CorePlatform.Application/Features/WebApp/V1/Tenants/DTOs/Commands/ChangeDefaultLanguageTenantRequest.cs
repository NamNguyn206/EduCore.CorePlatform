using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;

public class ChangeDefaultLanguageTenantRequest : IdRequest
{
    public string LanguageCode { get; init; } = default!;
}
