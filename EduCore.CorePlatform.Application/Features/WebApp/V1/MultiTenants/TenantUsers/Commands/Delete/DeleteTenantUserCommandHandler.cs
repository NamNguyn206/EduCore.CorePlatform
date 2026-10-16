using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.Delete;

public sealed class DeleteTenantUserCommandHandler: IRequestHandler<DeleteTenantUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public DeleteTenantUserCommandHandler(IUnitOfWork unitOfWork,
                                        ICurrentUser currentUser,
                                        ICurrentTenant currentTenant,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(DeleteTenantUserCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;
        if (tenantId == null)
        {
            return Result.Failure(ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        // =========================
        // KHÔNG CHO XÓA CHÍNH MÌNH
        // =========================
        if (request.RequestData.UserId == _currentUser.UserId)
        {
            return Result.Failure(ErrorCodeConstants.YouCannotRemoveYourselfFromTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.YouCannotRemoveYourselfFromTenant));
        }

        // =========================
        // LOAD USER TENANT
        // =========================
        var repo = _unitOfWork.Repository<UserTenant>();

        var spec = new TenantUserByUserIdAndTenantIdForDeleteSpec(_currentTenant, request.RequestData.UserId);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
            return Result.Failure(ErrorCodeConstants.UserNotInTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotInTenant));

        // =========================
        // KHÔNG CHO XÓA TENANT ADMIN CUỐI CÙNG
        // =========================
        var specTenantAdminCount = new TenantAdminCountForDeleteSpec(_currentTenant);
        var totalTenantAdmins = await repo.CountAsync(specTenantAdminCount, cancellationToken);

        if (entity.IsTenantAdmin && totalTenantAdmins <= 1)
        {
            return Result.Failure(ErrorCodeConstants.CannotRemoveTheLastTenantAdmin, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CannotRemoveTheLastTenantAdmin));
        }

        // =========================
        // KHÔNG CHO XÓA SYSTEM ADMIN
        // =========================
        if (entity.User.UserType == UserType.SuperAdmin)
        {
            return Result.Failure(ErrorCodeConstants.CannotRemoveSuperAdmin, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CannotRemoveSuperAdmin));
        }

        // =========================
        // DOMAIN LOGIC
        // =========================
        entity.LeaveTenant();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

