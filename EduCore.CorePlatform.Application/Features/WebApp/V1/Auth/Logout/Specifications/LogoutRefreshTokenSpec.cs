using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Logout.Specifications;

public sealed class LogoutRefreshTokenSpec
    : Specification<EduCore.CorePlatform.Domain.Entities.Identity.RefreshToken>
{
    public LogoutRefreshTokenSpec(string token)
    {
        AndCriteria(x => x.Token == token);
        EnableTracking();
    }
}
