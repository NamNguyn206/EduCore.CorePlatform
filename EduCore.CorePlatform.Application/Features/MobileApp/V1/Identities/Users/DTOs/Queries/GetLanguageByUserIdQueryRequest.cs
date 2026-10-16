using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Queries;

public class GetLanguageByUserIdQueryRequest
{
    public Guid UserId { get; set; } = default!;
}
