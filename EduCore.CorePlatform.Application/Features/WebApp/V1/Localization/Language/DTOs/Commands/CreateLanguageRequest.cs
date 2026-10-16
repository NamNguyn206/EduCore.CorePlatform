using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Commands;

public class CreateLanguageRequest
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
}
