using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.InviteExistingUserToTenant;

public sealed class InviteExistingUserToTenantCommandHandler
    : IRequestHandler<InviteExistingUserToTenantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public InviteExistingUserToTenantCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ICurrentUser currentUser,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        InviteExistingUserToTenantCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId!.Value;

        if (tenantId == Guid.Empty)
        {
            return Result.Failure(
                ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        var repoUser = _unitOfWork.Repository<User>();
        var repoRole = _unitOfWork.Repository<Role>();

        // 1️ Load users
        var users = await repoUser.ListAsync(new UsersByIdsWithDetailsForInviteExistingUserToTenantSpec(request.RequestData.UserIds), cancellationToken);

        if (!users.Any())
        {
            return Result.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        // 2️ Load roles
        var roles = await repoRole.ListAsync(new RolesByIdsForInviteExistingUserToTenantSpec(request.RequestData.RoleIds), cancellationToken);

        var invitedCount = 0;

        foreach (var user in users)
        {
            // ❌ Không cho mời System Admin
            if (user.UserType == UserType.SuperAdmin)
                continue;

            // ❌ Nếu đã thuộc tenant rồi thì bỏ qua
            var alreadyInTenant = user.UserTenants.Any(x => x.TenantId == tenantId && x.LeftAt == null);

            if (alreadyInTenant)
                continue;

            // 3️ Tạo UserTenant
            user.AddToTenant(tenantId, request.RequestData.IsTenantAdmin, request.RequestData.RoleIds);

            invitedCount++;
        }

        if (invitedCount == 0)
        {
            return Result.Failure(ErrorCodeConstants.UserAlreadyInTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserAlreadyInTenant));
        }

        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Success();
    }
}