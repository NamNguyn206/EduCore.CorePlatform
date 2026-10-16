using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications.Authorization.PermissionAuthorization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Infrastructure.Authorization;

public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionAuthorizationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // =========================================
        // 0. AUTHENTICATION CHECK
        // =========================================
        if (context.User?.Identity?.IsAuthenticated != true)
            return;

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return;

        var userId = Guid.Parse(userIdClaim.Value);

        var userTenantRoleRepo = _unitOfWork.Repository<UserTenantRole>();

        // =========================================
        // 1. SYSTEM ADMIN → BYPASS ALL
        // =========================================

        var isSuperAdmin = await userTenantRoleRepo.AnyAsync(new SuperAdminSpec(userId), CancellationToken.None);

        if (isSuperAdmin)
        {
            context.Succeed(requirement);
            return;
        }

        // =========================================
        // 2. NO TENANT CONTEXT → DENY
        // =========================================
        var tenantIdClaim = context.User.FindFirst("tenant_id");
        if (tenantIdClaim == null)
            return;

        var tenantId = Guid.Parse(tenantIdClaim.Value);

        // =========================================
        // 3. TENANT ADMIN → FULL ACCESS (THIS TENANT)
        // =========================================
        var isTenantAdmin = await userTenantRoleRepo.AnyAsync(new TenantAdminSpec(userId, tenantId), CancellationToken.None);

        if (isTenantAdmin)
        {
            context.Succeed(requirement);
            return;
        }

        // =========================================
        // 4. NORMAL PERMISSION CHECK
        // =========================================
        var hasPermission = await userTenantRoleRepo.AnyAsync(new UserPermissionSpec(
                userId,
                tenantId,
                requirement.Permission),
            CancellationToken.None);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}
