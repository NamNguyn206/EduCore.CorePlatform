using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static EduCore.CorePlatform.Domain.Common.Constants.Auth.PermissionConstants;


namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Commands.SwitchTenant;

public sealed class SwitchTenantCommandHandler : IRequestHandler<SwitchTenantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public SwitchTenantCommandHandler(IUnitOfWork unitOfWork,
                                        ICurrentTenant currentTenant,
                                        ICurrentUser currentUser,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(SwitchTenantCommand request, CancellationToken cancellationToken)
    {
        var repoUser = _unitOfWork.Repository<User>();
        var repoLanguage = _unitOfWork.Repository<Language>();

        var userSpec = new UserByIdForSwitchTenantSpec(_currentUser.UserId);

        var user = await repoUser.FirstOrDefaultAsync(userSpec, cancellationToken);

        if (user == null)
        {
            return Result.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        var userTenant = user.UserTenants.FirstOrDefault(x => x.TenantId == request.RequestData.TenantId 
                                                            && x.LeftAt == null);

        if (userTenant == null)
        {
            return Result.Failure(ErrorCodeConstants.TenantAccessDenied, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantAccessDenied));
        }

        user.SetLastTenant(request.RequestData.TenantId);

        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Success();
    }
}
