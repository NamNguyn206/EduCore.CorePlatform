using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Domain.Common.Constants.System;

public static class RequestHeaderTypeConstants
{
    public const string XTenantId = "X-Tenant-Id";
    public const string AcceptLanguage = "Accept-Language";    
}
