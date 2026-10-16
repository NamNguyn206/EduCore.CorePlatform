using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using EduCore.CorePlatform.Infrastructure.Authorization;

namespace EduCore.CorePlatform.Infrastructure.Authorization.Scanning;


public sealed class PermissionDiscoveryService : IPermissionDiscoveryService
{
    public IReadOnlyCollection<string> DiscoverPermissions()
    {
        var permissions = new HashSet<string>();

        var controllers = global::System.Reflection.Assembly.GetExecutingAssembly()
                                                            .GetTypes()
                                                            .Where(t =>
                                                                typeof(ControllerBase).IsAssignableFrom(t) &&
                                                                !t.IsAbstract);

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods(
                global::System.Reflection.BindingFlags.Instance |
                global::System.Reflection.BindingFlags.Public |
                global::System.Reflection.BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes<AuthorizePermissionAttribute>().FirstOrDefault();

                if (attr != null)
                {
                    var permission = attr.Policy!
                        .Replace(AuthorizePermissionAttribute.POLICY_PREFIX, "");

                    permissions.Add(permission);
                }
            }
        }

        return permissions;
    }
}
