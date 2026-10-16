using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Commands;

public class UpdateLanguageRequest : UpdateRequest
{
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
}
