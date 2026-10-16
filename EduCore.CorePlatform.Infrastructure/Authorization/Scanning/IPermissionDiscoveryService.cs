using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Authorization.Scanning;

public interface IPermissionDiscoveryService
{
    IReadOnlyCollection<string> DiscoverPermissions();
}
