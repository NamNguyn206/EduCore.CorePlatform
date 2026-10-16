using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Enums.System.Webhook;

public enum WebhookEventType
{
    TenantCreated = 1,
    TenantUpdated = 2,
    TenantDeleted = 3,
    UserCreated = 4,
    UserUpdated = 5,
    UserDeleted = 6,
    RoleCreated = 7,
    RoleUpdated = 8,
    RoleDeleted = 9,
    PermissionChanged = 10,
    AuditLogCreated = 11,
    CustomFieldChanged = 12,
    EntityCreated = 13,
    EntityUpdated = 14,
    EntityDeleted = 15
}