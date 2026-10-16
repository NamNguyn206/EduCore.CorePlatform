using EduCore.CorePlatform.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.System.SystemMessages.DTOs;

public class SystemMessageDto
{
    public string Code { get; set; }
    public string Content { get; set; }
    public string Language { get; set; }
    public string Type { get; set; }
}
