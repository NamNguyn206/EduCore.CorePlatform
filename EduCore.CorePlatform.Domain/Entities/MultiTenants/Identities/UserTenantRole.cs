using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;

public class UserTenantRole : AggregateRoot<Guid>
{
    public Guid UserTenantId { get; private set; }
    public Guid RoleId { get; private set; }
    public PartnershipStatus Status { get; private set; }

    public Guid? ApprovedByUserId { get; private set; }

    public DateTime? ApprovedAt { get; private set; }

    public string? RejectReason { get; private set; }

    // Navigation properties
    public UserTenant UserTenant { get; private set; } = default!;
    public Role Role { get; private set; } = default!;

    private UserTenantRole() { }

    public UserTenantRole(Guid userTenantId, Guid roleId)
    {
        UserTenantId = userTenantId;
        RoleId = roleId;

        Status = PartnershipStatus.Pending;
    }
    public void Approve(Guid approvedBy)
    {
        Status = PartnershipStatus.Approved;

        ApprovedByUserId = approvedBy;

        ApprovedAt = DateTime.UtcNow;

        RejectReason = null;
    }

    public void Reject(
        Guid approvedBy,
        string reason)
    {
        Status = PartnershipStatus.Rejected;

        ApprovedByUserId = approvedBy;

        ApprovedAt = DateTime.UtcNow;

        RejectReason = reason;
    }

    public void Suspend()
    {
        Status = PartnershipStatus.Suspended;
    }
    public void Terminate()
    {
        Status = PartnershipStatus.Terminated;
    }
}
